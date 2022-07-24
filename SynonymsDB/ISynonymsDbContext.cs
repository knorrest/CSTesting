using Microsoft.EntityFrameworkCore;

namespace SynonymsDB
{
    public interface ISynonymsDbContext
    {
        public DbSet<Synonym> Synonyms { get; set; }
        public DbSet<Word> Words { get; set; }
    }
}
