using SynonymsDB;

namespace SynonymsAPI.Interfaces
{
    public interface ISynonymsService
    {
        public List<WordDto> Get();
        public List<WordDto> SearchByWord(string word);
        public bool Add(string word, List<string> synonym);
    }
}
