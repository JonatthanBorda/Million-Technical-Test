using AutoMapper;
using Million.Application.Mapping;

namespace Million.Tests
{
    public static class TestFixture
    {
        public static IMapper CreateMapper()
        {
            var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            cfg.AssertConfigurationIsValid();
            return cfg.CreateMapper();
        }
    }
}
