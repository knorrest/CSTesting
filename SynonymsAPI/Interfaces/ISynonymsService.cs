using SynonymsAPI.DTOs;
using SynonymsDB;

namespace SynonymsAPI.Interfaces
{
    public interface ISynonymsService
    {
        Task<List<WordSynonym>> Get(string word);
        Task<bool> Add(string word, string synonym);
    }
}
