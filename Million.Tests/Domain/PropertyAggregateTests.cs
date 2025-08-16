using FluentAssertions;
using Million.Domain.Properties;
using Million.Domain.Properties.ValueObjects;
using System.Linq;

namespace Million.Tests.Domain
{
    public class PropertyAggregateTests
    {
        [Test]
        public void ChangePrice_updates_price_and_adds_trace()
        {
            var address = new Address("1 Main", "NYC", "NY", "10001");
            var price = new Money(100_000, "USD");
            var ownerId = Guid.NewGuid();

            var p = new Property(
                "Depto",
                address,
                price,
                "CODE-1",
                2010,
                ownerId,
                2);

            p.ChangePrice(new Money(120_000, "USD"), new DateOnly(2024, 1, 1), 0);

            p.Price.Amount.Should().Be(120_000);
            p.Traces.Should().HaveCount(1);
            p.Traces.Last().Value.Should().Be(120_000);
        }
    }
}
