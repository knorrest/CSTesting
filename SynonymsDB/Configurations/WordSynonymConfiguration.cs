using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace SynonymsDB.Configurations
{
    public class WordSynonymConfiguration : IEntityTypeConfiguration<WordSynonym>
    {
        public virtual void Configure(EntityTypeBuilder<WordSynonym> builder)
        {
            builder.HasKey(ws => new { ws.WordId, ws.SynonymId });
            builder.ToTable("wordSynonyms");
            builder.Property(x => x.WordId).HasColumnName("word_id").IsRequired();
            builder.Property(x => x.SynonymId).HasColumnName("synonym_id").IsRequired();


            builder.HasOne(ws => ws.Word)
                    .WithMany(s => s.WordSynonyms)
                    .HasForeignKey(ws => ws.WordId);

            builder.HasOne(ws => ws.Synonym)
                    .WithMany(w => w.WordSynonyms)
                    .HasForeignKey(ws => ws.SynonymId);
        }
    }
}
