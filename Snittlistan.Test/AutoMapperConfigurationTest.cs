using AutoMapper;
using Castle.Windsor;
using NUnit.Framework;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Infrastructure.Installers;

namespace Snittlistan.Test
{
    [TestFixture]
    public class AutoMapperConfigurationTest
    {
        [Test]
        public void VerifyConfiguration()
        {
            var container = new WindsorContainer().Install(new AutoMapperInstaller());
            AutoMapperConfiguration.Configure(container);
            Mapper.AssertConfigurationIsValid();
        }
    }
}