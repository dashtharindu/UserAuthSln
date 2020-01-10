using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuth.Data;
using UserAuth.Dtos;
using UserAuth.Models;

namespace UserAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]UserForRegisterDto getuser)
        {
            getuser.Username = getuser.Username.ToLower();
            bool result = await _repo.IsUserExist(getuser.Username);
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
                Username = getuser.Username
            };

            var createUser = await _repo.Register(myUser, getuser.Password);

            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm]UserForRegisterDto getuser)
        {
            getuser.Username = getuser.Username.ToLower();

            var result = await _repo.Login(getuser.Username,getuser.Password);

            if (result==null)
            {
                return Unauthorized("Invalid Username or Password");
            }

            return Ok($"Welcome {result.Username}");
            //var tokenHandler = new JwtSecurityTokenHandler();

        }

    }
}