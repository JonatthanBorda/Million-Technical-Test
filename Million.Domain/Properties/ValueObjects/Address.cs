using Million.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Domain.Properties.ValueObjects
{
    /// <summary>
    /// Value Object que representa una dirección completa.
    /// </summary>
    public sealed record Address
    {
        /// <summary>Calle y número.</summary>
        public string Street { get; }

        /// <summary>Ciudad.</summary>
        public string City { get; }

        /// <summary>Estado/Provincia.</summary>
        public string State { get; }

        /// <summary>Código postal.</summary>
        public string Zip { get; }

        /// <summary>
        /// Construye una dirección válida.
        /// </summary>
        public Address(string street, string city, string state, string zip)
        {
            Guard.AgainstNullOrWhiteSpace(street, nameof(street));
            Guard.AgainstNullOrWhiteSpace(city, nameof(city));
            Guard.AgainstNullOrWhiteSpace(state, nameof(state));
            Guard.AgainstNullOrWhiteSpace(zip, nameof(zip));

            Street = street;
            City = city;
            State = state;
            Zip = zip;
        }

        /// <summary>Representación legible de la dirección.</summary>
        public override string ToString() => $"{Street}, {City}, {State} {Zip}";
    }
}
