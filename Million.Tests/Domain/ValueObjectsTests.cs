using FluentAssertions;
using Million.Domain.Common;
using Million.Domain.Properties.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Tests.Domain
{
    public class ValueObjectsTests
    {
        [Test]
        public void Address_rejects_blank_fields()
        {
            var act = () => new Address("", "City", "ST", "00000");
            act.Should().Throw<DomainException>();
        }

        [Test]
        public void Money_rejects_non_positive_or_bad_currency()
        {
            Action a = () => new Money(0, "USD");
            a.Should().Throw<DomainException>();

            Action b = () => new Money(10, "US");
            b.Should().Throw<DomainException>();
        }
    }
}
