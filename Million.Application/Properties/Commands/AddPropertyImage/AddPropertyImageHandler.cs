using MediatR;
using Million.Application.Abstractions.Persistence;
using Million.Application.Common;
using Million.Application.DTOs;
using AutoMapper;
using Million.Domain.Common;

namespace Million.Application.Properties.Commands.AddPropertyImage;

public sealed class AddPropertyImageHandler
    : IRequestHandler<AddPropertyImageCommand, Result<PropertyDTO>>
{
    private readonly IPropertyRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AddPropertyImageHandler(IPropertyRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<PropertyDTO>> Handle(AddPropertyImageCommand req, CancellationToken ct)
    {
        var property = await _repo.GetByIdAsync(req.PropertyId, ct);
        if (property is null)
            return Result<PropertyDTO>.Failure(new Error("property.not_found", $"Propiedad '{req.PropertyId}' no encontrada."));

        property.AddImage(req.File, req.Enabled);

        await _uow.SaveChangesAsync(ct);

        return Result.Success(_mapper.Map<PropertyDTO>(property));
    }
}
