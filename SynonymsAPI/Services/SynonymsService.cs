using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SynonymsAPI.Interfaces;
using SynonymsDB;
using System.Linq;

namespace SynonymsAPI.Services
{
    public class SynonymsService : ISynonymsService
    {
        private const string wordListCacheKey = "wordList";
        private readonly IMemoryCache _cache;
        private List<Word> _words;

        public SynonymsService(IMemoryCache memoryCache)
        {
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            if (!_cache.TryGetValue(wordListCacheKey, out List<Word> words))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(560))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                _cache.Set(wordListCacheKey, SeedData.initialWords, cacheEntryOptions);
                _words = SeedData.initialWords;
            }
            else _words = words;
        }
        public List<WordDto> Get()
        {
            var wordsDto = _words.Select(x => GetByWord(x.WordString)).ToList();
            return wordsDto;
        }
        public WordDto? GetByWord(string word)
        {
            //Get all synonyms for that word
            var wordWithSynonyms = _words.FirstOrDefault(x => x.WordString == word);

            //Word not found
            if (wordWithSynonyms == null) return null;
            var wordDto = new WordDto() { WordString = word };

            //Get synonyms
            wordDto.Synonyms = _words.Where(x => wordWithSynonyms.SynonymIds.Contains(x.Id)).Select(x => x.WordString).ToList();

            return wordDto;
        }

        public List<WordDto> SearchByWord(string word)
        {
            //Get all synonyms for that word
            var wordsWithSynonyms = _words.Where(x => x.WordString.ToLower().StartsWith(word.ToLower()));
            var wordsList = new List<WordDto>();

            //Words not found
            if (!wordsWithSynonyms.Any()) return wordsList;

            //Get synonyms
            foreach (var wordWithSynonyms in wordsWithSynonyms)
            {
                var wordDto = new WordDto() { WordString = wordWithSynonyms.WordString };
                wordDto.Synonyms = _words.Where(x => wordWithSynonyms.SynonymIds.Contains(x.Id)).Select(x => x.WordString).ToList();
                wordsList.Add(wordDto);
            }
            return wordsList;
        }


        private List<string> GetSynonymWords(Word initialWord)
        {
            var synonymWords = _words.Where(w => initialWord.SynonymIds.Contains(w.Id));
            var synonyms = synonymWords.Select(x => x.WordString).ToList();

            foreach (var word in synonymWords)
            {
                synonyms.AddRange(GetSynonymWords(word));
            }
            return synonyms;
        }

        public WordDto? GetByWordR(string word)
        {
            //Get all synonyms for that word
            var wordWithSynonyms = _words.FirstOrDefault(x => x.WordString == word);

            //Word not found
            if (wordWithSynonyms == null) return null;
            var wordDto = new WordDto() { WordString = word };

            //Get synonyms
            wordDto.Synonyms = GetSynonymWords(wordWithSynonyms);

            return wordDto;
        }
        public bool Add(string word, List<string> synonyms)
        {
            try
            {

                //Remove all empty strings, and get Distinct values
                synonyms.RemoveAll(s => string.IsNullOrWhiteSpace(s));
                synonyms = synonyms.Distinct().ToList();

                if (word == null || synonyms.Count == 0) return false;

                //NextId is a simulation of a db ID
                var nextId = _words.Count + 1;

                //If the word that is a synonym already exists, get the ID. If not, add it
                var synonymsFromCache = _words.Where(x => synonyms.Contains(x.WordString.ToLower())).ToList();
                var synonymIds = synonymsFromCache.Select(x => x.Id).ToList();
                var newSynonymIds = new List<long>();

                //Get synonyms that are not already in cache
                var synonymsNotInCache = synonyms.Where(s => !synonymsFromCache.Select(x => x.WordString).Contains(s));

                //Add those synonyms
                foreach (var synonym in synonymsNotInCache)
                {
                    _words.Add(new Word()
                    {
                        Id = nextId,
                        WordString = synonym.ToLower(),
                        SynonymIds = synonymIds,
                    });
                    newSynonymIds.Add(nextId);
                    nextId++;
                }
                var allSynonymIds = synonymIds.Concat(newSynonymIds).ToList();

                //Check if word already exists
                var existingWord = _words.FirstOrDefault(x => x.WordString == word.ToLower());

                //If it doesn't, add the word and synonyms
                if (existingWord == null)
                {
                    //Add new word and bind synonyms
                    _words.Add(new Word() { Id = nextId, WordString = word.ToLower(), SynonymIds = allSynonymIds.ToList() });
                    allSynonymIds.Add(nextId);
                }
                else
                {
                    allSynonymIds.Add(existingWord.Id);

                    //If it does, hook only synonyms that aren't already in the list
                    _words.Where(x => x.Id == existingWord.Id).Select(x =>
                    {
                        x.SynonymIds = x.SynonymIds.Concat(newSynonymIds).ToList();
                        return x;
                    }).ToList();

                    allSynonymIds = allSynonymIds.Concat(existingWord.SynonymIds).ToList();
                }

                allSynonymIds = allSynonymIds.ToList();

                //Update newly added synonyms to have the references to all words
                _words.Where(x => allSynonymIds.Contains(x.Id)).Select(x =>
                {
                    var idsToAdd = allSynonymIds.Where(id => id != x.Id);
                    var synonymIds = x.SynonymIds.Concat(idsToAdd).Distinct().ToList();
                    x.SynonymIds = synonymIds;
                    return x;
                }).ToList();

                //Update the cache
                _cache.Set(wordListCacheKey, _words);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
