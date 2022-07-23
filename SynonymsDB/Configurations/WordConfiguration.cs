using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace SynonymsDB.Configurations
{
    public class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public virtual void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.ToTable("words");
            builder.HasKey(x => x.Id); 
            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.WordString).HasColumnName("word").IsRequired();
        }
    }
}
