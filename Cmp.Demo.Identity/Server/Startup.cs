using Cmp.Demo.Identity.Server.Configuration;
using IdentityServer3.Core.Configuration;
using Owin;

namespace Cmp.Demo.Identity.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                SigningCertificate = Certificate.Get(),
                Factory = Factory.Configure("IdentityServer")
            };

            app.UseIdentityServer(options);
        }
    }
}