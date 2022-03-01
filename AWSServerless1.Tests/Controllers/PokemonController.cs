using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PokemonAPI.Common;
using PokemonAPI.Controllers;
using PokemonAPI.Ifx;
using PokemonAPI.Managers;
using System.Collections.Generic;
using System.Net;
using Xunit;
using PokemonAPI.Tests.TestHelpers;
namespace PokemonAPI.Tests.Controllers
{
    public class PokemonControllerTest
    {
        [Fact]
        public async void IfAPokemonIsAvailable_Return200()
        {
            //Arrange

            var httpContextWrapper = A.Fake<IHttpContextWrapper>();
            var logger = A.Fake<ILogger<PokemonController>>();
            var generateClassWithFake = new GenerateClassWithFakes();
            var controller = generateClassWithFake.Generate<PokemonController>(httpContextWrapper);
            A.CallTo(() => httpContextWrapper.GetAPIGatewayProxyRequest(A<HttpContext>.Ignored)).Returns(new Amazon.Lambda.APIGatewayEvents.APIGatewayProxyRequest()
            {
                QueryStringParameters = new Dictionary<string, string>()
                {
                    {"name", "Charizard" }
                }
            });      

            //Act
            var result = await controller.GetPokemonDetails();

            //Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }
    }
}
