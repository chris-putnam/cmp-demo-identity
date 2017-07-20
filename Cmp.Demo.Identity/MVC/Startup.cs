using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using IdentityServer3.Core;
using System.Web.Helpers;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using IdentityModel.Client;
using System;
using System.Linq;

namespace Cmp.Demo.Identity.MVC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44366/",
                ClientId = "mvc",
                RedirectUri = "https://localhost:44376/",
                Scope = "openid profile roles sampleApi",
                ResponseType = "id_token token",
                SignInAsAuthenticationType = "Cookies",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                        {
                            var nid = new ClaimsIdentity(
                                n.AuthenticationTicket.Identity.AuthenticationType,
                                Constants.ClaimTypes.GivenName,
                                Constants.ClaimTypes.Role);

                            // get userinfo data
                            var userInfoClient = new UserInfoClient(
                                new Uri(n.Options.Authority + "/connect/userinfo"),
                                n.ProtocolMessage.AccessToken);

                            var userInfo = await userInfoClient.GetAsync();
                            userInfo.Claims.ToList().ForEach(ui => nid.AddClaim(new Claim(ui.Item1, ui.Item2)));

                            // keep the id_token for logout
                            nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                            // add access token for sample API
                            nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

                            // keep track of access token expiration
                            nid.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));

                            // add some other app specific claim
                            nid.AddClaim(new Claim("app_specific", "some data"));

                            n.AuthenticationTicket = new AuthenticationTicket(
                                nid,
                                n.AuthenticationTicket.Properties);
                        }
                }
            });
        }
    }
}