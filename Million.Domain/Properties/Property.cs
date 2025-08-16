using Million.Domain.Abstractions;
using Million.Domain.Common;
using Million.Domain.Events;
using Million.Domain.Properties.ValueObjects;

namespace Million.Domain.Properties
{
    /// <summary>
    /// Raíz de agregado que representa una propiedad.
    /// </summary>
    public sealed class Property : AggregateRoot
    {
        /// <summary>Nombre de la propiedad.</summary>
        public string Name { get; private set; } = default!;

        /// <summary>Dirección física.</summary>
        public Address Address { get; private set; } = default!;

        /// <summary>Precio actual.</summary>
        public Money Price { get; private set; } = Money.Usd(1);

        /// <summary>Código interno (único por Owner).</summary>
        public string CodeInternal { get; private set; } = default!;

        /// <summary>Año de construcción.</summary>
        public int Year { get; private set; }

        /// <summary>Cantidad de habitaciones (opcional).</summary>
        public int? Rooms { get; private set; }

        /// <summary>Referencia al dueño por Id.</summary>
        public Guid OwnerId { get; private set; }

        // Colección de imágenes administradas por la raíz del agregado.
        private readonly List<PropertyImage> _images = new();

        /// <summary>Imágenes asociadas (sólo lectura).</summary>
        public IReadOnlyCollection<PropertyImage> Images => _images.AsReadOnly();

        // Colección de registros históricos de precio/venta.
        private readonly List<PropertyTrace> _traces = new();

        /// <summary>Histórico de precio/venta (sólo lectura).</summary>
        public IReadOnlyCollection<PropertyTrace> Traces => _traces.AsReadOnly();

        /// <summary>
        /// Ctor privado para EF Core (requiere parameterless) y para forzar uso del factory Create.
        /// </summary>
        private Property() { }

        /// <summary>
        /// Ctor privado con validaciones. Se usa desde el factory Create.
        /// </summary>
        public Property(
            string name,
            Address address,
            Money price,
            string codeInternal,
            int year,
            Guid ownerId,
            int? rooms)
        {
            //Validaciones básicas:
            Guard.AgainstNullOrWhiteSpace(name, nameof(name));
            Guard.AgainstNullOrWhiteSpace(codeInternal, nameof(codeInternal));
            Guard.AgainstOutOfRange(year, 1800, DateTime.UtcNow.Year + 1, nameof(year));
            if (ownerId == Guid.Empty)
                throw new DomainException("OwnerId no puede ser vacío.");
            if (rooms is < 0)
                throw new DomainException("Rooms no puede ser negativo.");

            Name = name;
            Address = address ?? throw new DomainException("Address es requerido.");
            Price = price ?? throw new DomainException("Price es requerido.");
            CodeInternal = codeInternal;
            Year = year;
            OwnerId = ownerId;
            Rooms = rooms;
        }

        /// <summary>
        /// Factory estático recomendado (más expresivo para la capa Application).
        /// El orden de parámetros está alineado con tu handler:
        /// name, address, price, codeInternal, year, rooms, ownerId.
        /// </summary>
        public static Property Create(
            string name,
            Address address,
            Money price,
            string codeInternal,
            int year,
            int? rooms,
            Guid ownerId)
            => new(name, address, price, codeInternal, year, ownerId, rooms);

        /// <summary>
        /// Actualiza datos principales de la propiedad.
        /// </summary>
        public void Update(string? name, Address address, int year, int? rooms)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            Guard.AgainstOutOfRange(year, 1800, DateTime.UtcNow.Year + 1, nameof(year));
            if (rooms is < 0)
                throw new DomainException("Rooms no puede ser negativo.");

            Address = address ?? throw new DomainException("Address es requerido.");
            Year = year;
            Rooms = rooms;
        }

        /// <summary>
        /// Agrega una imagen.
        /// </summary>
        public void AddImage(string file, bool enabled = true)
        {
            Guard.AgainstNullOrWhiteSpace(file, nameof(file));
            var image = new PropertyImage(Id, file, enabled);
            _images.Add(image);
        }

        /// <summary>
        /// Cambia el precio de la propiedad y registra una traza; emite evento de dominio.
        /// </summary>
        public PropertyTrace ChangePrice(Money newPrice, DateOnly dateSale, decimal tax)
        {
            var oldPrice = Price.Amount;

            //Actualiza el precio:
            Price = newPrice;

            //Crea la traza:
            var trace = new PropertyTrace(Id, dateSale, Name, newPrice.Amount, tax);
            _traces.Add(trace);

            //Evento de dominio:
            Raise(new PriceChangedDomainEvent(Id, oldPrice, newPrice.Amount));

            return trace;
        }

        /// <summary>
        /// Para Tests.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="price"></param>
        /// <param name="codeInternal"></param>
        /// <param name="year"></param>
        /// <param name="ownerId"></param>
        /// <param name="rooms"></param>
        /// <returns></returns>
        //public static Property Create(
        //    string name,
        //    Address address,
        //    Money price,
        //    string codeInternal,
        //    int year,
        //    Guid ownerId,
        //    int? rooms = null)
        //{
        //    return new Property(name, address, price, codeInternal, year, ownerId, rooms);
        //}
    }
}
