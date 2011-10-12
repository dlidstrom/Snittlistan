using AutoMapper;
using Castle.Windsor;
using Snittlistan.Infrastructure.AutoMapper;
using Snittlistan.Installers;
using Xunit;

namespace Snittlistan.Test
{
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
