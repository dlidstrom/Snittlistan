namespace Snittlistan.Test
{
    using AutoMapper;
    using Castle.Windsor;
    using Infrastructure.AutoMapper;
    using Infrastructure.Installers;
    using Xunit;

    public class AutoMapperConfigurationTest
    {
        [Fact]
        public void VerifyConfiguration()
        {
            var container = new WindsorContainer().Install(new AutoMapperInstaller());
            AutoMapperConfiguration.Configure(container);
            Mapper.AssertConfigurationIsValid();
        }
    }
}