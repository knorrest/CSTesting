using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynonymsDB.Configurations
{
    public class SynonymConfiguration : IEntityTypeConfiguration<Synonym>
    {
        public virtual void Configure(EntityTypeBuilder<Synonym> builder)
        {
            builder.ToTable("synonyms");
            builder.HasKey(x => x.Id).HasName("id");
            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.SynonymString).HasColumnName("synonym").IsRequired();
        }
    }
}
