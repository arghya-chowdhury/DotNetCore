using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginModel.Username)
                    };
                    var userIdentity = new ClaimsIdentity(claims, "login");
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
            await _accClient.GetAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var articles = await _client.GetAllAsync();
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
                var result = await _client.PostAsync(article);
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
            var article = await _client.GetAsync(id);
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                var result = await _client.PutAsync(article.Id, article);
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
            var article = await _client.GetAsync(id);
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Article article)
        {
            var result = await _client.DeleteAsync(article.Id);
            if (result.StatusCode == 200)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return new StatusCodeResult(result.StatusCode);
            }
        }
    }
}