using Microsoft.EntityFrameworkCore;
using SynonymsAPI.DTOs;
using SynonymsAPI.Interfaces;
using SynonymsDB;

namespace SynonymsAPI.Services
{
    public class SynonymsService : ISynonymsService
    {
        private readonly ISynonymsDbContext _context;
        public SynonymsService(ISynonymsDbContext context)
        {
            _context = context;
        }
        public async Task<List<WordSynonym>> Get(string word)
        {
            return await _context.WordSynonyms.Where(x => x.Word.WordString == word).ToListAsync();
        }
        public async Task<bool> Add(string word, string synonym)
        {
            var wordFromDb = await _context.Words.FirstOrDefaultAsync(x => x.WordString == word);
            wordFromDb.WordSynonyms.Add(new WordSynonym { WordId = wordFromDb.Id, Synonym = synonym})

        }
    }
}
