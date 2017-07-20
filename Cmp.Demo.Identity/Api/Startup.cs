using IdentityServer3.AccessTokenValidation;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Web.Http;

namespace Cmp.Demo.Identity.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44366/",
                RequiredScopes = new[] { "sampleApi" },
                PreserveAccessToken = true
            });

            // web api configuration
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}