namespace Million.WebApi.Contracts.Requests
{
    /// <summary>
    /// Modelo para actualización de Propiedad.
    /// </summary>
    public sealed class UpdatePropertyRequest
    {
        public string? Name { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Zip { get; set; } = default!;
        public int Year { get; set; }
        public int? Rooms { get; set; }
    }
}
