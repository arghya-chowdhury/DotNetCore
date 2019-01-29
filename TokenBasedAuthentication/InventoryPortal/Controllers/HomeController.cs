using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryPortal.Controllers
{
    
    public class HomeController : Controller
    {
        private AccountClient _accClient;
        private InventoryClient _client;

        public HomeController(AccountClient accClient, InventoryClient client)
        {
            _accClient = accClient;
            _client = client;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accClient.PostAsync(loginModel);
                if(result.StatusCode==200)
                {
                    var token = new StreamReader(result.Stream).ReadToEnd();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginModel.Username),
                        new Claim(ClaimTypes.Authentication, token)
                    };
                    var userIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    return new RedirectToActionResult("Index", "Home", null);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var token = RetriveTokenFromContext();
            var articles = await _client.GetAllAsync(token);
            return View(articles);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                var token = RetriveTokenFromContext();
                var result = await _client.PostAsync(article, token);
                if (result.StatusCode == 200)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return new StatusCodeResult(result.StatusCode);
                }
            }
            return View(article);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = RetriveTokenFromContext();
            var article = await _client.GetAsync(id, token);
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                var token = RetriveTokenFromContext();
                var result = await _client.PutAsync(article.Id, article, token);
                if (result.StatusCode == 200)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return new StatusCodeResult(result.StatusCode);
                }
            }
            return View(article);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = RetriveTokenFromContext();
            var article = await _client.GetAsync(id, token);
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Article article)
        {
            var token = RetriveTokenFromContext();
            var result = await _client.DeleteAsync(article.Id, token);
            if (result.StatusCode == 200)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return new StatusCodeResult(result.StatusCode);
            }
        }

        private string RetriveTokenFromContext()
        {
            var authClaim = HttpContext.User.Identities.SelectMany(i => i.Claims)
                .FirstOrDefault(c => c.Type == ClaimTypes.Authentication);
            return authClaim?.Value?.Replace("\"", string.Empty);
        }
    }
}