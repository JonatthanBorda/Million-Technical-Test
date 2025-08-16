using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Infrastructure.Persistence
{
    /// <summary>
    /// Permite crear el DbContext en tiempo de diseño para comandos 'dotnet ef'.
    /// </summary>
    public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MillionDbContext>
    {
        public MillionDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var cs = configuration.GetConnectionString("MillionConnection")
                ?? throw new InvalidOperationException("No se encontró 'ConnectionStrings:MillionConnection' en appsettings.json.");

            var options = new DbContextOptionsBuilder<MillionDbContext>()
                .UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(MillionDbContext).Assembly.FullName))
                .Options;

            return new MillionDbContext(options);
        }
    }
}
