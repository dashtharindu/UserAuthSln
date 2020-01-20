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
    public class DogController : ControllerBase //todo - change this name to freeDogsController
    {
        private readonly IDogRepository _repo;

        public DogController(IDogRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dog>> Get(int id)
        {
            var dog =await _repo.GetDog(id);
            return Ok(dog);
        }

        [HttpGet]
        public async Task<ActionResult<DogsToShopList>> Get()
        {
            var dog = await _repo.GetDogs();
            return Ok(dog);
        }
    }
}