namespace Million.WebApi.Contracts.Requests
{
    /// <summary>
    /// Modelo para creación de Propiedad.
    /// </summary>
    public sealed class CreatePropertyRequest
    {
        public string Name { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Zip { get; set; } = default!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "USD";
        public string CodeInternal { get; set; } = default!;
        public int Year { get; set; }
        public int? Rooms { get; set; }
        public Guid OwnerId { get; set; }
    }
}
