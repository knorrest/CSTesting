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
        public async Task<WordSynonymsDto> GetAsync(string word)
        {
            var wordSynonyms = await _context.Synonyms.Where(x=> x.Word.WordString == word).Select(x=>x.SynonymString).ToListAsync();
            return new WordSynonymsDto()
            {
                Word = word,
                Synonyms = wordSynonyms
            };
        }
        public async Task<bool> AddAsync(string word, string synonym)
        {
            var wordFromDb = await _context.Words.FirstOrDefaultAsync(x => x.WordString == word);
            if (wordFromDb == null)
            {
               // throw new NotFoundException("")
            }
            wordFromDb.Synonyms.Add(new Synonym() { SynonymString = synonym });
            return true;
        }
    }
}
