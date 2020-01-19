using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuthorizeWithGithub.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Octokit;
using Octokit.Internal;

namespace AuthorizeWithGithub.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = new GithubModel();
            if (User.Identity.IsAuthenticated)
            {
                model.GitHubName = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                model.GitHubLogin = User.FindFirst(c => c.Type == "urn:github:login")?.Value;
                model.GitHubUrl = User.FindFirst(c => c.Type == "urn:github:url")?.Value;
                model.GitHubAvatar = User.FindFirst(c => c.Type == "urn:github:avatar")?.Value;

                string accessToken = await HttpContext.GetTokenAsync("access_token");
                var github = new GitHubClient(new ProductHeaderValue("OAuthIntegrationTest"), // productheadervalue will be your application name on github OAuth
                    new InMemoryCredentialStore(new Credentials(accessToken)));
                model.Repositories = await github.Repository.GetAllForCurrent();
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
