using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cmp.Demo.Identity.MVC.Controllers
{
    public class CallApiController : Controller
    {
        // GET: CallApi/UserCredentials
        public async Task<ActionResult> Index()
        {
            var user = User as ClaimsPrincipal;
            var token = user.FindFirst("access_token").Value;
            var result = await CallApi(token);

            ViewBag.Json = result;
            return View("ShowApiResult");
        }

        private async Task<string> CallApi(string token)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);

            var json = await client.GetStringAsync("https://localhost:44310/identity");
            return JArray.Parse(json).ToString();
        }
    }
}