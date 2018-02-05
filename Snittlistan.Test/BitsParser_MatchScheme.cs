using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Test
{
    [TestFixture]
    public class BitsParser_MatchScheme
    {
        private ParseMatchSchemeResult matchScheme;

        [SetUp]
        public void SetUp()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("Snittlistan.Test.BitsResult.VärtansIK-matchScheme.html"))
            {
                Debug.Assert(stream != null, nameof(stream) + " != null");
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    matchScheme = BitsParser.ParseMatchScheme(content);
                }
            }
        }

        [Test]
        public void ParsesMatchScheme()
        {
            // Assert
            Assert.That(matchScheme, Is.Not.Null);
        }
    }
}