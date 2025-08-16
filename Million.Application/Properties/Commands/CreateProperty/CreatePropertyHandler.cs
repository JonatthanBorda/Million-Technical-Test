using AutoMapper;
using MediatR;
using Million.Application.Abstractions.Persistence;
using Million.Application.Common;
using Million.Application.DTOs;
using Million.Domain.Common;
using Million.Domain.Owners;
using Million.Domain.Properties;
using Million.Domain.Properties.ValueObjects;

namespace Million.Application.Properties.Commands.CreateProperty;

public sealed class CreatePropertyHandler
    : IRequestHandler<CreatePropertyCommand, Result<PropertyDTO>>
{
    private readonly IPropertyReadRepository _readRepo;
    private readonly IPropertyRepository _writeRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IOwnerReadRepository _owners;

    public CreatePropertyHandler(
        IPropertyReadRepository readRepo,
        IPropertyRepository writeRepo,
        IUnitOfWork uow,
        IMapper mapper,
        IOwnerReadRepository owners)
    {
        _readRepo = readRepo;
        _writeRepo = writeRepo;
        _uow = uow;
        _mapper = mapper;
        _owners = owners;
    }

    public async Task<Result<PropertyDTO>> Handle(CreatePropertyCommand req, CancellationToken ct)
    {
        //1. Normaliza el CodeInternal:
        var code = (req.CodeInternal ?? string.Empty).Trim();

        //2. Valida que exista el Owner:
        if (!await _owners.ExistsAsync(req.OwnerId, ct))
        {
            return Result.Failure<PropertyDTO>(
                new Error("owner.not_found",
                          $"No existe un propietario con Id '{req.OwnerId}'."));
        }

        //3. Valida duplicado de CodeInternal:
        if (await _readRepo.ExistsCodeInternalAsync(code, ct))
        {
            return Result.Failure<PropertyDTO>(
                new Error("property.duplicate_codeinternal",
                          $"Ya existe una propiedad con CodeInternal '{code}'."));
        }

        //4. Crea la propiedad:
        var address = new Address(req.Street, req.City, req.State, req.Zip);
        var price = new Money(req.Price, req.Currency);

        var property = Property.Create(
            req.Name,
            address,
            price,
            code,
            req.Year,
            req.Rooms,
            req.OwnerId);

        _writeRepo.Add(property);
        await _uow.SaveChangesAsync(ct);

        //Mapea a DTO y devuelve éxito:
        var dto = _mapper.Map<PropertyDTO>(property);
        return Result.Success(dto);
    }
}
