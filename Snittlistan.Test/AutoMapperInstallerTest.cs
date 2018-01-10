using System.Linq;
using AutoMapper;
using Castle.Core.Internal;
using Castle.Windsor;
using NUnit.Framework;
using Snittlistan.Web.Infrastructure.Installers;

namespace Snittlistan.Test
{
    [TestFixture]
    public class AutoMapperInstallerTest
    {
        private readonly IWindsorContainer container;

        public AutoMapperInstallerTest()
        {
            container = new WindsorContainer()
                .Install(new AutoMapperInstaller());
        }

        [Test]
        public void AllProfilesImplementProfile()
        {
            var allHandlers = InstallerTestHelper.GetAllHandlers(container);
            var profileHandlers = InstallerTestHelper.GetHandlersFor(typeof(Profile), container);
            Assert.That(allHandlers, Is.Not.Empty);
            Assert.That(profileHandlers, Is.EqualTo(allHandlers));
        }

        [Test]
        public void AllProfilesAreRegistered()
        {
            var allProfiles = InstallerTestHelper.GetPublicClassesFromApplicationAssembly(c => c.Is<Profile>());
            var registeredProfiles = InstallerTestHelper.GetImplementationTypesFor(typeof(Profile), container);
            Assert.That(registeredProfiles, Is.EqualTo(allProfiles));
        }

        [Test]
        public void AllProfilesExposeProfileAsService()
        {
            var profilesWithWrongService = InstallerTestHelper.GetHandlersFor(typeof(Profile), container)
                .Where(c => !c.ComponentModel.Services.SequenceEqual(new[] { typeof(Profile) }))
                .ToArray();
            Assert.That(profilesWithWrongService, Is.Empty);
        }
    }
}