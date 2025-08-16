using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Abstractions
{
    /// <summary>Abstracción para testear lógica dependiente de tiempo.</summary>
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }

    public sealed class SystemDateTime : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
