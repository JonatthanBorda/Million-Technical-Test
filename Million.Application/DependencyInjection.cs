using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Million.Application.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application
{
    /// <summary>
    /// Registro de servicios de la capa Application.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //MediatR:
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            //AutoMapper:
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //FluentValidation:
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //Pipeline Behavior: corre validaciones antes de cada Handler:
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //Utilidades (fecha/hora del sistema):
            services.AddSingleton<Abstractions.IDateTime, Abstractions.SystemDateTime>();

            return services;
        }
    }
}
