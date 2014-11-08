using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using Raven.Imports.Newtonsoft.Json;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Test.ApiControllers
{
    [TestFixture]
    public class RegisterMatch_Get : WebApiIntegrationTest
    {
        private HttpResponseMessage responseMessage;
        private string content;

        [Test]
        public void ShouldRegisterPendingResult()
        {
            // Act
            responseMessage = Client.GetAsync("http://temp.uri/api/registermatch").Result;
            var httpContent = responseMessage.Content;

            // Assert
            Assert.That(httpContent, Is.Not.Null);
            content = httpContent.ReadAsStringAsync()
                                     .Result;
            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void ShouldReturnInfoAboutRegisteredMatches()
        {
            var expected = new[]
            {
                new
                {
                    id = 654321
                }
            };
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override void OnSetUp(Castle.Windsor.IWindsorContainer container)
        {
            // Arrange
            Transact(session =>
            {
                var roster = new Roster(2012, 1, 654321, "FIF", "A", "Bowl-O-Rama", "AIK F", new DateTime(2012, 1, 1), false);
                session.Store(roster);
            });
        }
    }
}