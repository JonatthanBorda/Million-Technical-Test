using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

using Million.Application.Abstractions.Persistence;
using Million.Application.DTOs;
using Million.Application.Mapping;
using Million.Application.Properties.Commands.CreateProperty;
using Million.Domain.Properties;

namespace Million.Tests.Application.Handlers
{
    public class CreatePropertyHandlerTests
    {
        private IMapper _mapper = null!;

        [SetUp]
        public void SetUp()
        {
            var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            cfg.AssertConfigurationIsValid();
            _mapper = cfg.CreateMapper();
        }

        [Test]
        public async Task Returns_not_found_when_owner_is_missing()
        {
            var owners = new Mock<IOwnerReadRepository>();
            owners.Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(false);

            var read = new Mock<IPropertyReadRepository>();
            var write = new Mock<IPropertyRepository>();
            var uow = new Mock<IUnitOfWork>();

            var h = new CreatePropertyHandler(read.Object, write.Object, uow.Object, _mapper, owners.Object);

            var cmd = new CreatePropertyCommand(
                "Prop", "1 Main", "NYC", "NY", "10001",
                1000, "USD", "CODE-1", 2020, 2, Guid.NewGuid());

            var res = await h.Handle(cmd, CancellationToken.None);

            res.IsSuccess.Should().BeFalse();
            res.Error!.Code.Should().Be("owner.not_found");
            write.Verify(x => x.Add(It.IsAny<Property>()), Times.Never);
            uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Returns_conflict_when_codeinternal_is_duplicated()
        {
            var owners = new Mock<IOwnerReadRepository>();
            owners.Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);

            var read = new Mock<IPropertyReadRepository>();
            // Unicidad GLOBAL -> método con (string code, CancellationToken)
            read.Setup(x => x.ExistsCodeInternalAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var write = new Mock<IPropertyRepository>();
            var uow = new Mock<IUnitOfWork>();

            var h = new CreatePropertyHandler(read.Object, write.Object, uow.Object, _mapper, owners.Object);

            var cmd = new CreatePropertyCommand(
                "Prop", "1 Main", "NYC", "NY", "10001",
                1000, "USD", "DUP-1", 2020, 2, Guid.NewGuid());

            var res = await h.Handle(cmd, CancellationToken.None);

            res.IsSuccess.Should().BeFalse();
            res.Error!.Code.Should().Be("property.duplicate_codeinternal");
            write.Verify(x => x.Add(It.IsAny<Property>()), Times.Never);
            uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Creates_property_ok()
        {
            var owners = new Mock<IOwnerReadRepository>();
            owners.Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);

            var read = new Mock<IPropertyReadRepository>();
            read.Setup(x => x.ExistsCodeInternalAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var write = new Mock<IPropertyRepository>();
            var uow = new Mock<IUnitOfWork>();

            var h = new CreatePropertyHandler(read.Object, write.Object, uow.Object, _mapper, owners.Object);

            var cmd = new CreatePropertyCommand(
                "Prop", "1 Main", "NYC", "NY", "10001",
                1000, "USD", "CODE-OK", 2020, 2, Guid.NewGuid());

            var res = await h.Handle(cmd, CancellationToken.None);

            res.IsSuccess.Should().BeTrue();
            res.Value!.Name.Should().Be("Prop");
            write.Verify(x => x.Add(It.IsAny<Property>()), Times.Once);
            uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
