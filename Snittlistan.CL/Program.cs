namespace Snittlistan.CL
{
    using System;
    using System.IO;
    using System.Text;
    using Castle.Windsor;
    using Infrastructure.Installers;
    using Models;
    using Raven.Client;

    static class Program
    {
        static void Main(string[] args)
        {
            using (var container = new WindsorContainer().Install(new RavenInstaller(DocumentStoreMode.InMemory)))
            using (var store = container.Resolve<IDocumentStore>())
            {
                var jsonSerializer = store.Conventions.CreateSerializer();
                string password;
                Console.Write("Enter password ('q' exits): ");
                while ((password = Console.ReadLine()) != "q")
                {
                    var user = new User(string.Empty, string.Empty, string.Empty, password);
                    var builder = new StringBuilder();
                    jsonSerializer.Serialize(new StringWriter(builder), user);
                    Console.WriteLine(builder.ToString());
                    Console.Write("Enter password ('q' exits): ");
                }
            }
        }
    }
}
