using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace Cmp.Demo.Identity.Server.Configuration
{
    public class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "MVC Client",
                    ClientId = "mvc",
                    Flow = Flows.Implicit,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44376/"
                    },
                    AllowAccessToAllScopes = true,
                    RequireConsent = false
                },
                new Client
                {
                    Enabled = true,
                    ClientName = "SPA Client",
                    ClientId = "spa",
                    Flow = Flows.Implicit,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:4200/",
                        "http://localhost:4200/silent-renew.html"
                    },
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "email",
                        "roles",
                        "sampleApi"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4200"
                    },
                    RequireConsent = false
                }
            };
        }
    }
}