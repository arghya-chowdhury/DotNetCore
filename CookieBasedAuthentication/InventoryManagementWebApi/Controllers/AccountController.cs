using InventoryManagementWebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventoryManagementWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await HttpContext.SignOutAsync();
            return new OkResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(LoginModel loginModel)
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
                return new OkResult();
            }
            return new BadRequestResult();
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
