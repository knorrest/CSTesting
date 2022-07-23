using SynonymsAPI.DTOs;
using SynonymsDB;

namespace SynonymsAPI.Interfaces
{
    public interface ISynonymsService
    {
        Task<WordSynonymsDto> GetAsync(string word);
        Task<bool> AddAsync(string word, string synonym);
    }
}
