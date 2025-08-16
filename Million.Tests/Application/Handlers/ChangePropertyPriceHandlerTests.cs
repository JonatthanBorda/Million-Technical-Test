using AutoMapper;
using FluentAssertions;
using Moq;
using Million.Application.Abstractions.Persistence;
using Million.Application.Properties.Commands.ChangePropertyPrice;
using Million.Domain.Properties;
using Million.Domain.Properties.ValueObjects;

namespace Million.Tests.Application.Handlers
{
    public class ChangePropertyPriceHandlerTests
    {
        private readonly IMapper _mapper = TestFixture.CreateMapper();

        [Test]
        public async Task Returns_not_found_when_property_not_exists()
        {
            var repo = new Mock<IPropertyRepository>();
            repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Property?)null);

            var uow = new Mock<IUnitOfWork>();
            var h = new ChangePropertyPriceHandler(repo.Object, uow.Object, _mapper);

            var res = await h.Handle(
                new ChangePropertyPriceCommand(Guid.NewGuid(), 100, "USD", DateOnly.FromDateTime(DateTime.UtcNow), 0),
                CancellationToken.None);

            res.IsSuccess.Should().BeFalse();
            res.Error!.Code.Should().Be("property.not_found");
        }

        [Test]
        public async Task Updates_price_and_saves()
        {
            var address = new Address("1 Main", "NYC", "NY", "10001");
            var price = new Money(100, "USD");
            var entity = new Property("Prop", address, price, "C-1", 2020, Guid.NewGuid(), 2);

            var repo = new Mock<IPropertyRepository>();
            repo.Setup(x => x.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var uow = new Mock<IUnitOfWork>();

            var h = new ChangePropertyPriceHandler(repo.Object, uow.Object, _mapper);

            var res = await h.Handle(
                new ChangePropertyPriceCommand(entity.Id, 150, "USD", DateOnly.FromDateTime(DateTime.UtcNow), 0),
                CancellationToken.None);

            res.IsSuccess.Should().BeTrue();

            entity.Price.Amount.Should().Be(150m);
            entity.Traces.Should().ContainSingle(t => t.Value == 150m);

            uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
