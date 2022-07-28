using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SynonymsAPI;
using SynonymsAPI.Services;
using SynonymsDB;

namespace Test
{
    public class SynonymsServiceTest
    {
        private SynonymsService _synonymsService;
        private List<Word> _initialWords;

        [SetUp]
        public void Setup()
        {
            _initialWords = SeedData.initialWords;
            
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            _synonymsService = new SynonymsService(memoryCache);
        }

        [Test]
        public void Get_ShouldReturnAllWords()
        {
            var result = _synonymsService.Get();

            Assert.That(result, Has.Count.EqualTo(_initialWords.Count));
        }

        [Test]
        public void GetByWord_WordHouse_ShouldReturnSameWordCount()
        {
            const string word = "house";
            var wordSynonyms = _initialWords.FirstOrDefault(x => x.WordString == word);

            var result = _synonymsService.GetByWord(word);

            Assert.That(result.Synonyms, Has.Count.EqualTo(wordSynonyms.SynonymIds.Count));
        }

        [Test]
        public void SearchByWord_WordH_ShouldReturnSameWordCount()
        {
            const string word = "h";
            var searchedResults = _initialWords.Where(x => x.WordString.StartsWith(word)).ToList();

            var result = _synonymsService.SearchByWord(word);

            Assert.That(result, Has.Count.EqualTo(searchedResults.Count));
        }

        [Test]
        public void GetByWord_WordHouse_ShouldReturnExactSynonyms()
        {
            const string word = "house";
            var wordSynonyms = _initialWords.FirstOrDefault(x => x.WordString == word);
            var synonymList = _initialWords.Where(x => wordSynonyms.SynonymIds.Contains(x.Id)).ToList();

            var result = _synonymsService.GetByWord(word);

            Assert.That(result.Synonyms.Count, Is.EqualTo(wordSynonyms.SynonymIds.Count));

            var results = result.Synonyms.ToList();
            Assert.That(results.Count, Is.EqualTo(synonymList.Count));
            foreach (var synonym in synonymList)
            {
                Assert.That(results, Does.Contain(synonym.WordString));
            }
        }


        [TestCase("house")]
        [TestCase("home")]
        [TestCase("accommodation")]
        [TestCase("job")]
        [TestCase("occupation")]
        public void SearchByWord_WordHouse_ShouldReturnExactSynonyms(string word)
        {
            var wordSynonyms = _initialWords.FirstOrDefault(x => x.WordString == word);
            var synonymList = _initialWords.Where(x => wordSynonyms.SynonymIds.Contains(x.Id)).ToList();

            var result = _synonymsService.SearchByWord(word);

            Assert.That(result, Has.Count.EqualTo(1));

            var results = result.FirstOrDefault().Synonyms.ToList();
            Assert.That(results, Has.Count.EqualTo(synonymList.Count));

            foreach (var synonym in synonymList)
            {
                Assert.That(results, Does.Contain(synonym.WordString));
            }
        }

        [Test]
        public void SearchByWord_WordHouseCaseSensitive_ShouldReturnSameResult()
        {
            const string wordLowercase = "house";
            const string wordUppercase = "HOUSE";
            var wordSynonymsLowercase = _initialWords.FirstOrDefault(x => x.WordString == wordLowercase);

            var resultLowercase = _synonymsService.GetByWord(wordLowercase);
            var resultUppercase = _synonymsService.GetByWord(wordUppercase);

            CollectionAssert.AreEqual(resultLowercase.Synonyms, resultUppercase.Synonyms);
        }

        [Test]
        public void AddWord_NewWordNewSynonyms_ShouldAddAllWords()
        {
            var newWord = "conversation";
            var synonyms = new List<string>() { "chat", "TALK" };
            var initialWordsCount = _initialWords.Count + synonyms.Count;

            var addResult = _synonymsService.Add(newWord, synonyms);
            var newArray = _synonymsService.Get();
            Assert.Multiple(() =>
            {
                Assert.That(addResult, Is.True);
                Assert.That(initialWordsCount, Is.EqualTo(newArray.Count));
            });
        }

        [Test]
        public void AddWord_NewWordExistingSynonyms_ShouldAddAllWords()
        {
            var newWord = "cabin";
            var synonyms = new List<string>() { "house", "home" };
            var initialWordsCount = _initialWords.Count + 1;

            var addResult = _synonymsService.Add(newWord, synonyms);
            var newArray = _synonymsService.Get();

            Assert.Multiple(() =>
            {
                Assert.That(addResult, Is.True);
                Assert.That(initialWordsCount, Is.EqualTo(newArray.Count));
            });
        }

        [Test]
        public void AddWord_EmptyAndDuplicatedSynonyms_ShouldSkipEmptyAndDuplicate()
        {
            var newWord = "conversation";
            var synonyms = new List<string>() { "talk", "talk", " ", "    " };
            var initialWordsCount = _initialWords.Count + 2;

            var addResult = _synonymsService.Add(newWord, synonyms);
            var newArray = _synonymsService.Get();

            Assert.Multiple(() =>
            {
                Assert.That(addResult, Is.True);
                Assert.That(initialWordsCount, Is.EqualTo(newArray.Count));
            });
        }
    }
}