using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Infrastructure.Persistence.Configurations
{
    /// <summary>Configuración de PropertyTrace.</summary>
    public sealed class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
    {
        public void Configure(EntityTypeBuilder<PropertyTrace> builder)
        {
            builder.ToTable("PropertyTrace");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DateSale)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Value)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.Tax)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasIndex(x => x.PropertyId);
            builder.HasIndex(x => x.DateSale);
        }
    }
}
