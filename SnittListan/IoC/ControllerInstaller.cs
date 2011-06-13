using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using System.Web.Mvc;

namespace SnittListan.IoC
{
	public class ControllerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
					AllTypes
						.FromThisAssembly()
						.BasedOn<IController>()
						.Configure(c => c.LifeStyle.Transient.Named(c.Implementation.Name))
				);
		}
	}
}