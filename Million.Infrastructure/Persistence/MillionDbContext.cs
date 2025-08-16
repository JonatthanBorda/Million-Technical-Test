using Microsoft.EntityFrameworkCore;
using Million.Domain.Owners;
using Million.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Infrastructure.Persistence
{
    /// <summary>
    /// DbContext principal de Million.
    /// </summary>
    public sealed class MillionDbContext : DbContext
    {
        public MillionDbContext(DbContextOptions<MillionDbContext> options) : base(options) { }

        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
        public DbSet<PropertyTrace> PropertyTraces => Set<PropertyTrace>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Aplica todas las clases que implementen IEntityTypeConfiguration<T> en este ensamblado:
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MillionDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
