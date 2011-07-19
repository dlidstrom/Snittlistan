using AutoMapper;
using Castle.Windsor;
using SnittListan.Infrastructure;
using SnittListan.Installers;
using Xunit;

namespace SnittListan.Test
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
