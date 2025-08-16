using AutoMapper;
using Million.Application.DTOs;
using Million.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Mapping
{
    /// <summary>
    /// Perfil de mapeos de Entidad a DTO.
    /// </summary>
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Mapping para listado de imagenes:
            CreateMap<PropertyImage, PropertyImageItemDTO>()
                .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
                .ForCtorParam("File", o => o.MapFrom(s => s.File))
                .ForCtorParam("Enabled", o => o.MapFrom(s => s.Enabled));

            //Mapping para listado de trazas:
            CreateMap<PropertyTrace, PropertyTraceItemDTO>()
                .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
                .ForCtorParam("DateSale", o => o.MapFrom(s => s.DateSale))
                .ForCtorParam("Name", o => o.MapFrom(s => s.Name))
                .ForCtorParam("Value", o => o.MapFrom(s => s.Value))
                .ForCtorParam("Tax", o => o.MapFrom(s => s.Tax));

            //Mapping para listado de propiedades:
            CreateMap<Property, PropertyListItemDTO>()
                .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
                .ForCtorParam("Name", o => o.MapFrom(s => s.Name))
                .ForCtorParam("Street", o => o.MapFrom(s => s.Address.Street))
                .ForCtorParam("City", o => o.MapFrom(s => s.Address.City))
                .ForCtorParam("State", o => o.MapFrom(s => s.Address.State))
                .ForCtorParam("CodeInternal", o => o.MapFrom(s => s.CodeInternal))
                .ForCtorParam("Price", o => o.MapFrom(s => s.Price.Amount))
                .ForCtorParam("Currency", o => o.MapFrom(s => s.Price.Currency))
                .ForCtorParam("Year", o => o.MapFrom(s => s.Year))
                .ForCtorParam("Rooms", o => o.MapFrom(s => s.Rooms));

            //Mapping para detalle de propiedad:
            CreateMap<Property, PropertyDTO>()
                .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
                .ForCtorParam("Name", o => o.MapFrom(s => s.Name))
                .ForCtorParam("Street", o => o.MapFrom(s => s.Address.Street))
                .ForCtorParam("City", o => o.MapFrom(s => s.Address.City))
                .ForCtorParam("State", o => o.MapFrom(s => s.Address.State))
                .ForCtorParam("Zip", o => o.MapFrom(s => s.Address.Zip))
                .ForCtorParam("Price", o => o.MapFrom(s => s.Price.Amount))
                .ForCtorParam("Currency", o => o.MapFrom(s => s.Price.Currency))
                .ForCtorParam("CodeInternal", o => o.MapFrom(s => s.CodeInternal))
                .ForCtorParam("Year", o => o.MapFrom(s => s.Year))
                .ForCtorParam("Rooms", o => o.MapFrom(s => s.Rooms))
                .ForCtorParam("OwnerId", o => o.MapFrom(s => s.OwnerId))
                .ForCtorParam("Images", o => o.MapFrom(s => s.Images));
        }
    }
}
