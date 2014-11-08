using System.Net;
using Xunit;

namespace Snittlistan.Test.ApiControllers
{
    public class RegisterMatch_Get : WebApiIntegrationTest
    {
        [Fact]
        public async void ShouldRegisterPendingResult()
        {
            // Arrange

            // Act
            var responseMessage = await Client.GetAsync("http://temp.uri/api/registermatch");

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }
}