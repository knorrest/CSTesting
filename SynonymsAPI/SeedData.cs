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
                    WordString = "House",
                    SynonymIds = new List<long>() { 2, 3, 4 }
                },
                new Word()
                {
                    Id = 2,
                    WordString = "Home",
                    SynonymIds = new List<long>() { 1, 3, 4 }
                },
                 new Word()
                {
                    Id = 3,
                    WordString = "Accommodation",
                    SynonymIds = new List<long>() { 1, 2, 4 }
                },
                  new Word()
                {
                    Id = 4,
                    WordString = "Property",
                    SynonymIds = new List<long>() { 1, 2, 3 }
                },
                  new Word()
                {
                    Id = 5,
                    WordString = "Job",
                    SynonymIds = new List<long>() { 6, 7, 8 }
                },
                 new Word()
                {
                    Id = 6,
                    WordString = "Occupation",
                    SynonymIds = new List<long>() { 5, 7, 8 }
                },
                  new Word()
                {
                    Id = 7,
                    WordString = "Position",
                    SynonymIds = new List<long>() { 5,6,8 }
                },
                    new Word()
                {
                    Id = 8,
                    WordString = "Career",
                    SynonymIds = new List<long>() {5, 6, 7}
                }
        };
    }
}
