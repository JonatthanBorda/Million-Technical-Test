using MediatR;
using Million.Application.Abstractions.Persistence;
using Million.Application.Common;
using Million.Application.DTOs;
using AutoMapper;
using Million.Domain.Common;
using Million.Domain.Properties.ValueObjects;

namespace Million.Application.Properties.Commands.ChangePropertyPrice;

public sealed class ChangePropertyPriceHandler
    : IRequestHandler<ChangePropertyPriceCommand, Result<PropertyDTO>>
{
    private readonly IPropertyRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ChangePropertyPriceHandler(IPropertyRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<PropertyDTO>> Handle(ChangePropertyPriceCommand req, CancellationToken ct)
    {
        var property = await _repo.GetByIdAsync(req.PropertyId, ct);
        if (property is null)
            return Result.Failure<PropertyDTO>(new Error("property.not_found",
                $"Propiedad '{req.PropertyId}' no encontrada."));

        var money = new Money(req.NewAmount, req.Currency);
        var date = req.DateSale ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var tax = req.Tax ?? 0m;

        var trace = property.ChangePrice(money, date, tax);

        _repo.AddTrace(trace);

        await _uow.SaveChangesAsync(ct);

        return Result.Success(_mapper.Map<PropertyDTO>(property));
    }
}
