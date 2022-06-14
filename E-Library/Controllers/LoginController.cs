using E_Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {

        private IConfiguration _config;
        private readonly appContext _context;
        public LoginController(IConfiguration config, appContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<string>> post([FromForm]string user_name , [FromForm] string password)
        {

            var user = _context.users.FirstOrDefault(u => u.user_name == user_name);
            if (user == null)
                return NotFound("User not found");

            if (user.password == password)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                var claims = new[] {
                    new Claim("rol" , user.member_type),
                    new Claim("id" , user.id.ToString())
                };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(20),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            if (user.password != password)
                return Unauthorized("Pass word wrong");

            return user_name;
        }
    }
}
