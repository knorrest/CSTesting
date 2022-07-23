using Microsoft.EntityFrameworkCore;

namespace SynonymsDB
{
    public class SynonymsDbContext : DbContext, ISynonymsDbContext
    {
        public DbSet<Synonym> Synonyms { get; set; }
        public DbSet<Word> Words { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SynonymsDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=SynonymsDatabase;Pooling=true;Integrated Security=true;");
    }
}
