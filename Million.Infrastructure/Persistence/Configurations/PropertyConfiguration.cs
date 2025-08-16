using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.Domain.Owners;
using Million.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Infrastructure.Persistence.Configurations
{
    /// <summary>Configuración de la entidad Property.</summary>
    public sealed class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("Property");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.Property(x => x.CodeInternal).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.CodeInternal);

            //CodeInternal único:
            builder.HasIndex(x => x.CodeInternal)
                   .IsUnique()
                   .HasDatabaseName("UX_Property_CodeInternal");

            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.OwnerId).IsRequired();

            //Address:
            builder.OwnsOne(x => x.Address, a =>
            {
                a.Property(p => p.Street).HasColumnName("Street").HasMaxLength(200).IsRequired();
                a.Property(p => p.City).HasColumnName("City").HasMaxLength(120).IsRequired();
                a.Property(p => p.State).HasColumnName("State").HasMaxLength(80).IsRequired();
                a.Property(p => p.Zip).HasColumnName("Zip").HasMaxLength(10).IsRequired();

                a.HasIndex(p => p.City);
                a.HasIndex(p => p.State);
            });

            //Price:
            builder.OwnsOne(x => x.Price, p =>
            {
                p.Property(m => m.Amount).HasColumnName("Price").HasColumnType("decimal(18,2)").IsRequired();
                p.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
            });

            //Relaciones 1:N (Images / Traces):
            builder.HasMany(x => x.Images).WithOne().HasForeignKey(pi => pi.PropertyId).OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(x => x.Images).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(x => x.Traces).WithOne().HasForeignKey(pt => pt.PropertyId).OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(x => x.Traces).UsePropertyAccessMode(PropertyAccessMode.Field);

            //Relación Owner (1:N)
            builder.HasOne<Owner>()
                   .WithMany()
                   .HasForeignKey(p => p.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
