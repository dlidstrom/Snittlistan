using AutoMapper;
using SnittListan.Infrastructure;
using Xunit;

namespace SnittListan.Test
{
	public class AutoMapperConfigurationTest
	{
		[Fact]
		public void VerifyConfiguration()
		{
			AutoMapperConfiguration.Configure();
			Mapper.AssertConfigurationIsValid();
		}
	}
}
