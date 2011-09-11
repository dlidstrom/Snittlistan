using System.Linq;
using AutoMapper;
using Castle.Core.Internal;
using Castle.Windsor;
using Snittlistan.Infrastructure;
using Snittlistan.Installers;
using Snittlistan.Models;
using Snittlistan.ViewModels;
using Xunit;

namespace Snittlistan.Test
{
	public class AutoMapperInstallerTest
	{
		private readonly IWindsorContainer container;

		public AutoMapperInstallerTest()
		{
			container = new WindsorContainer()
				.Install(new AutoMapperInstaller());
		}

		[Fact]
		public void AllProfilesImplementProfile()
		{
			var allHandlers = InstallerTestHelper.GetAllHandlers(container);
			var profileHandlers = InstallerTestHelper.GetHandlersFor(typeof(Profile), container);
			Assert.NotEmpty(allHandlers);
			Assert.Equal(allHandlers, profileHandlers);
		}

		[Fact]
		public void AllProfilesAreRegistered()
		{
			var allProfiles = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Is<Profile>());
			var registeredProfiles = InstallerTestHelper.GetImplementationTypesFor(typeof(Profile), container);
			Assert.Equal(allProfiles, registeredProfiles);
		}

		[Fact]
		public void AllProfilesExposeProfileAsService()
		{
			var profilesWithWrongService = InstallerTestHelper.GetHandlersFor(typeof(Profile), container)
				.Where(c => c.ComponentModel.Services.Any(s => s != typeof(Profile)))
				.ToArray();
			Assert.Empty(profilesWithWrongService);
		}
	}
}
