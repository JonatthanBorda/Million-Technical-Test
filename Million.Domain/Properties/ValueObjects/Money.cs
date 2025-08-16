using Million.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Properties.ValueObjects
{
    /// <summary>
    /// Value Object para manejar valores monetarios.
    /// </summary>
    public sealed record Money
    {
        /// <summary>Monto monetario positivo.</summary>
        public decimal Amount { get; }

        /// <summary>Código de moneda (COP/USD/EUR).</summary>
        public string Currency { get; }

        /// <summary>
        /// Crea una cantidad de dinero válida. Requiere monto > 0 y moneda de 3 letras.
        /// </summary>
        public Money(decimal amount, string currency)
        {
            Guard.AgainstNonPositive(amount, nameof(amount));
            Guard.AgainstNullOrWhiteSpace(currency, nameof(currency));
            if (currency.Length != 3)
                throw new DomainException("La moneda debe ser un código ISO-4217 de 3 letras.");

            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        /// <summary>Para montos en dólares.</summary>
        public static Money Usd(decimal amount) => new(amount, "USD");

        /// <summary>
        /// Crea una nueva instancia con mismo código de moneda y otro monto.
        /// </summary>
        public Money WithAmount(decimal newAmount) => new(newAmount, Currency);
    }
}
