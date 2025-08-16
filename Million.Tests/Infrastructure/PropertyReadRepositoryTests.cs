using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Million.Domain.Owners;
using Million.Domain.Properties;
using Million.Domain.Properties.ValueObjects;
using Million.Infrastructure.Persistence;
using Million.Infrastructure.Persistence.Repositories;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Million.Tests.Infrastructure
{
    public class PropertyReadRepositoryTests
    {
        private IMapper _mapper = TestFixture.CreateMapper();

        private MillionDbContext CreateContext(SqliteConnection cn)
        {
            var opt = new DbContextOptionsBuilder<MillionDbContext>()
                .UseSqlite(cn)
                .Options;

            var db = new MillionDbContext(opt);
            db.Database.EnsureCreated();
            return db;
        }

        [Test]
        public async Task ListAsync_filters_and_projects_to_dto()
        {
            using var cn = new SqliteConnection("Filename=:memory:");
            cn.Open();
            using var db = CreateContext(cn);

            //1. Owner real por la FK:
            var owner = new Owner(
                name: "Owner Test",
                address: "123 Main St",
                photo: null,
                birthday: new DateOnly(1990, 1, 1)
            );
            db.Owners.Add(owner);
            await db.SaveChangesAsync();

            //2. Seed de propiedades (ambas con imagen; p2 será el primero por price_desc):
            var p1 = new Property(
                name: "A",
                address: new Address("S1", "NYC", "NY", "10001"),
                price: new Money(300, "USD"),
                codeInternal: "C1",
                year: 2010,
                ownerId: owner.Id,
                rooms: 2);
            p1.AddImage("images/1.jpg", true);

            var p2 = new Property(
                name: "B",
                address: new Address("S2", "NYC", "NY", "10001"),
                price: new Money(500, "USD"),
                codeInternal: "C2",
                year: 2015,
                ownerId: owner.Id,
                rooms: 3);

            p2.AddImage("images/2.jpg", true);

            db.Properties.AddRange(p1, p2);
            await db.SaveChangesAsync();

            var repo = new PropertyReadRepository(db, _mapper);

            var page = await repo.ListAsync(
                city: "NYC", state: "NY",
                minPrice: 200, maxPrice: 600,
                minYear: null, maxYear: null,
                rooms: null, page: 1, pageSize: 10,
                orderBy: "price_desc", ct: default);

            page.TotalCount.Should().Be(2);
            page.Items.Should().BeInDescendingOrder(i => i.Price);
            page.Items.First().CodeInternal.Should().Be("C2");
            page.Items.First().Images.Should().NotBeEmpty();
        }
    }
}
