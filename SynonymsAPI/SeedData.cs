using SynonymsDB;

namespace SynonymsAPI
{
    public static class SeedData
    {
        public static List<Word> initialWords = new List<Word>()
                {
                new Word()
                {
                    Id = 1,
                    WordString = "house",
                    SynonymIds = new List<long>() { 2, 3, 4 }
                },
                new Word()
                {
                    Id = 2,
                    WordString = "home",
                    SynonymIds = new List<long>() { 1, 3, 4 }
                },
                 new Word()
                {
                    Id = 3,
                    WordString = "accommodation",
                    SynonymIds = new List<long>() { 1, 2, 4 }
                },
                  new Word()
                {
                    Id = 4,
                    WordString = "property",
                    SynonymIds = new List<long>() { 3, 1, 2 }
                },
                  new Word()
                {
                    Id = 5,
                    WordString = "job",
                    SynonymIds = new List<long>() { 6, 7, 8 }
                },
                 new Word()
                {
                    Id = 6,
                    WordString = "occupation",
                    SynonymIds = new List<long>() { 5, 7, 8 }
                },
                  new Word()
                {
                    Id = 7,
                    WordString = "position",
                    SynonymIds = new List<long>() { 5,6,8 }
                },
                    new Word()
                {
                    Id = 8,
                    WordString = "career",
                    SynonymIds = new List<long>() {5, 6, 7}
                }
        };
    }
}
