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
            var wordsDto = _words.Select(x => GetByWord(x.WordString)).OrderBy(x => x.WordString).ToList();
            return wordsDto;
        }

        public WordDto? GetByWord(string word)
        {
            //Get all synonyms for that word
            var wordWithSynonyms = _words.FirstOrDefault(x => x.WordString == word.ToLower());

            //Word not found
            if (wordWithSynonyms == null) return null;
            var wordDto = new WordDto
            {
                Id = wordWithSynonyms.Id,
                WordString = word,
                //Get synonyms
                Synonyms = _words.Where(x => wordWithSynonyms.SynonymIds.Contains(x.Id)).Select(x => x.WordString).ToList()
            };

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
                var wordDto = new WordDto
                {
                    Id = wordWithSynonyms.Id,
                    WordString = wordWithSynonyms.WordString,
                    Synonyms = _words.Where(x => wordWithSynonyms.SynonymIds.Contains(x.Id)).Select(x => x.WordString).ToList()
                };
                wordsList.Add(wordDto);
            }
            return wordsList.OrderBy(x => x.WordString).ToList();
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

        
        public bool Add(string word)
        {
            try
            {
                if (word == "A")
                {

                }
                else if (word.Contains("B"))
                {
                    if (word == "BA")
                    {

                    }
                    else if (word == "BB")
                    {

                    }
                }
                else if (word == "C")
                {

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddTest(string word)
        {
            try
            {
                if (word == "A")
                {

                }
                else if (word.Contains("B"))
                {
                    if (word == "BA")
                    {

                    }
                    else if (word == "BB")
                    {
                        if (word == "A")
                        {

                        }
                    }
                }
                else if (word == "C")
                {

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void UpdateAllSynonymsOfSynonyms(List<long> allSynonymIds)
        {
            _words.Where(x => allSynonymIds.Contains(x.Id)).Select(x =>
            {
                //Add list of new synonyms to existing list of synonyms
                var idsToAdd = x.SynonymIds.Concat(allSynonymIds.Where(id => id != x.Id));
                var synonymIds = idsToAdd.Distinct().ToList();
                x.SynonymIds = synonymIds;
                return x;
            }).ToList();
        }

        private void HookTheNewSynonymsToWord(Word existingWord, List<long> newSynonymIds)
        {
            _words.Where(x => x.Id == existingWord.Id).Select(x =>
            {
                x.SynonymIds = x.SynonymIds.Concat(newSynonymIds).ToList();
                return x;
            }).ToList();
        }

        private (List<long>, int) AddWordsToCache(int nextId, List<long> synonymIds, IEnumerable<string> synonymsNotInCache)
        {
            var newSynonymIds = new List<long>();
            foreach (var synonym in synonymsNotInCache)
            {
                _words.Add(new Word()
                {
                    Id = nextId,
                    WordString = synonym.ToLower(),
                    SynonymIds = synonymIds,
                });
                newSynonymIds.Add(nextId++);
            }

            return (newSynonymIds, nextId);
        }

    }
}
