using InventoryManagementWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        IConfiguration _configuration;
        public AccountController(IConfiguration configure)
        {
            _configuration = configure;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(LoginModel loginModel)
        {
            if (LoginUser(loginModel.Username, loginModel.Password))
            {
                var key = Encoding.ASCII.GetBytes(_configuration["Token:Secret"]);
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Issuer= _configuration["Token:Issuer"],
                    Audience= _configuration["Token:Audience"],
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, loginModel.Username)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return new OkObjectResult(tokenString);
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
