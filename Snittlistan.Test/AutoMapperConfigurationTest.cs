namespace Snittlistan.Test
{
    using AutoMapper;
    using Castle.Windsor;

    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Infrastructure.Installers;

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