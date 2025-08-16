using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Million.Application.Abstractions.Persistence;
using Million.Infrastructure.Persistence;
using Million.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Infrastructure
{
    /// <summary>
    /// Métodos de extensión para registrar Infrastructure en el contenedor de DI.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MillionConnection")
                ?? throw new InvalidOperationException("Connection string 'MillionConnection' no configurada.");

            services.AddDbContext<MillionDbContext>(opt =>
                opt.UseSqlServer(connectionString, sql =>
                {
                    //Asegura que las migraciones se generen en este ensamblado:
                    sql.MigrationsAssembly(typeof(MillionDbContext).Assembly.FullName);
                }));

            //Repos de escritura:
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();

            //Repos de lectura:
            services.AddScoped<IPropertyReadRepository, PropertyReadRepository>();
            services.AddScoped<IOwnerReadRepository, OwnerReadRepository>();

            return services;
        }
    }
}
