using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserAuth.Data;
using UserAuth.Dtos;
using UserAuth.Models;

namespace UserAuth.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto regUser)
        {
            regUser.Username = regUser.Username.ToLower();
            bool result = await _repo.IsUserExist(regUser.Username);
            if (result)
            {
                ModelState.AddModelError("Username","Username is already taken");
            }

            //validate api
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User myUser = new User
            {
                Username = regUser.Username
            };

            var createUser = await _repo.Register(myUser, regUser.Password);

            return StatusCode(201);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserForRegisterDto logUser)
        {
            logUser.Username = logUser.Username.ToLower();

            var result = await _repo.Login(logUser.Username,logUser.Password);

            if (result==null)
            {
                return Unauthorized("Invalid Username or Password");
            }

            //return Ok($"Welcome {result.Username}");

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes("Super Secret Key");
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]{
            //    new Claim(ClaimTypes.NameIdentifier,result.Id.ToString()),
            //    new Claim(ClaimTypes.Name,result.Username)
            //    }),
            //    Expires = DateTime.Now.AddDays(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            //        SecurityAlgorithms.HmacSha512Signature)

            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //var tokenString = tokenHandler.WriteToken(token);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,result.Id.ToString()),
                new Claim(ClaimTypes.Name,result.Username)
            };

            var key = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);



            return Ok(new { 
                token = tokenHandler.WriteToken(token)
            });

        }

    }
}