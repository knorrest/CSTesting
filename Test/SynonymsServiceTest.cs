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
        public void Get_ReturnsSameWordCount()
        {
            var result = _synonymsService.Get();

            Assert.That(result, Has.Count.EqualTo(_initialWords.Count));
        }

        [Test]
        public void GetByWord_WordHouse_ReturnsSameWordCount()
        {
            const string word = "house";
            var wordSynonyms = _initialWords.FirstOrDefault(x => x.WordString == word);

            var result = _synonymsService.GetByWord(word);

            Assert.That(result.Synonyms, Has.Count.EqualTo(wordSynonyms.SynonymIds.Count));
        }

        [Test]
        public void SearchByWord_WordH_ReturnsSameWordCount()
        {
            const string word = "h";
            var searchedResults = _initialWords.Where(x => x.WordString.StartsWith(word)).ToList();

            var result = _synonymsService.SearchByWord(word);

            Assert.That(result.Count, Is.EqualTo(searchedResults.Count));
        }

        [Test]
        public void GetByWord_WordHouse_ReturnsExactSynonyms()
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


        [Test]
        public void SearchByWord_WordHouse_ReturnsExactSynonyms()
        {
            const string word = "house";
            var wordSynonyms = _initialWords.FirstOrDefault(x => x.WordString == word);
            var synonymList = _initialWords.Where(x => wordSynonyms.SynonymIds.Contains(x.Id)).ToList();

            var result = _synonymsService.SearchByWord(word);

            Assert.That(result.Count, Is.EqualTo(1));

            var results = result.FirstOrDefault().Synonyms.ToList();
            Assert.That(results.Count, Is.EqualTo(synonymList.Count));
            foreach (var synonym in synonymList)
            {
                Assert.That(results, Does.Contain(synonym.WordString));
            }
        }

        [Test]
        public void SearchByWord_WordHouseCaseSensitive_ReturnsSameResult()
        {
            const string wordLowercase = "house";
            const string wordUppercase = "HOUSE";
            var wordSynonymsLowercase = _initialWords.FirstOrDefault(x => x.WordString == wordLowercase);

            var resultLowercase = _synonymsService.GetByWord(wordLowercase);
            var resultUppercase = _synonymsService.GetByWord(wordUppercase);

            CollectionAssert.AreEqual(resultLowercase.Synonyms, resultUppercase.Synonyms);
        }
    }
}