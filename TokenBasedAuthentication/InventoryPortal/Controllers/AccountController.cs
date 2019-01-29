using InventoryPortal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventoryPortal.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Home/Index/Account/Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (LoginUser(loginModel.Username, loginModel.Password))
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginModel.Username)
                    };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        private bool LoginUser(string username, string password)
        {
            if (username == "probetag" && password == "HH0GJJl?4iWY#d$O")
                return true;
            else
                return false;
        }
    }
}
