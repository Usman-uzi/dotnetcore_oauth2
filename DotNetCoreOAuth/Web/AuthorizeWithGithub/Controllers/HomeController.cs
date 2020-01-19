using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuthorizeWithGithub.Models;
using System.Security.Claims;

namespace AuthorizeWithGithub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new GithubModel();
            if (User.Identity.IsAuthenticated)
            {
                model.GitHubName = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                model.GitHubLogin = User.FindFirst(c => c.Type == "urn:github:login")?.Value;
                model.GitHubUrl = User.FindFirst(c => c.Type == "urn:github:url")?.Value;
                model.GitHubAvatar = User.FindFirst(c => c.Type == "urn:github:avatar")?.Value;
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
