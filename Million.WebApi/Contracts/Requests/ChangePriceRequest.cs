namespace Million.WebApi.Contracts.Requests
{
    /// <summary>
    /// Modelo para cambio de precio de Propiedad.
    /// </summary>
    public sealed class ChangePriceRequest
    {
        public decimal NewAmount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateOnly DateSale { get; set; }
        public decimal Tax { get; set; }
    }
}
