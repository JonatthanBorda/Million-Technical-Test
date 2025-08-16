using MediatR;
using Million.Application.Abstractions.Persistence;
using Million.Application.Common;
using Million.Application.DTOs;
using AutoMapper;
using Million.Domain.Common;
using Million.Domain.Properties.ValueObjects;

namespace Million.Application.Properties.Commands.UpdateProperty;

public sealed class UpdatePropertyHandler
    : IRequestHandler<UpdatePropertyCommand, Result<PropertyDTO>>
{
    private readonly IPropertyRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdatePropertyHandler(IPropertyRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<PropertyDTO>> Handle(UpdatePropertyCommand req, CancellationToken ct)
    {
        var property = await _repo.GetByIdAsync(req.PropertyId, ct);
        if (property is null)
            return Result.Failure<PropertyDTO>(new Error("property.not_found", $"Propiedad '{req.PropertyId}' no encontrada."));

        var address = new Address(req.Street, req.City, req.State, req.Zip);

        property.Update(req.Name, address, req.Year, req.Rooms);

        _repo.Update(property);
        await _uow.SaveChangesAsync(ct);

        return Result.Success(_mapper.Map<PropertyDTO>(property));
    }
}
