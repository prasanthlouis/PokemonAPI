using Amazon.Lambda.APIGatewayEvents;
using Microsoft.AspNetCore.Http;

namespace PokemonAPI.Common
{
    public interface IHttpContextWrapper {
        public APIGatewayProxyRequest GetAPIGatewayProxyRequest(HttpContext httpContext);
    }
    public class HttpContextWrapper : IHttpContextWrapper
    {
        public APIGatewayProxyRequest GetAPIGatewayProxyRequest(HttpContext httpContext)
        {
            return httpContext?.Items?[Amazon.Lambda.AspNetCoreServer.AbstractAspNetCoreFunction.LAMBDA_REQUEST_OBJECT] as APIGatewayProxyRequest;
        }
    }
}
