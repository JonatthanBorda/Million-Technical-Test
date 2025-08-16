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
    /// <summary>
    /// Configuración de de la entidad PropertyImage.
    /// </summary>
    public sealed class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> b)
        {
            b.ToTable("PropertyImage");
            b.HasKey(x => x.Id);

            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.File)
                .IsRequired()
                .HasMaxLength(1024);

            b.Property(x => x.Enabled)
                .HasDefaultValue(true);

            b.HasIndex(x => x.PropertyId);

            b.HasOne<Property>()
                .WithMany(p => p.Images)
                .HasForeignKey(x => x.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
