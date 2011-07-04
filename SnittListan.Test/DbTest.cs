using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Embedded;

namespace SnittListan.Test
{
	public abstract class DbTest : IDisposable
	{
		private readonly IDocumentStore store;

		public DbTest()
		{
			store = new EmbeddableDocumentStore
			{
				RunInMemory = true
			}.Initialize();
			Session = store.OpenSession();
		}

		public IDocumentSession Session { get; private set; }

		public void Dispose()
		{
			Session.Dispose();
		}
	}
}
