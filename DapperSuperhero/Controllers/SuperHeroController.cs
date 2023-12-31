using Dapper;
using DapperSuperhero.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperSuperhero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SuperHeroController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
        {
            using var connection = CreateConnection();
            var heroes = await connection.QueryAsync<SuperHero>("select * from superheroes");
            return Ok(heroes);
        }
        [HttpGet("{heroId}")]
        public async Task<ActionResult<SuperHero>> GetSuperHeroById(int heroId)
        {
            using var connection = CreateConnection();
            var hero = await connection.QueryFirstAsync<SuperHero>("select * from superheroes where Id = @Id",
                new {Id = heroId});
            return Ok(hero);
        }
    }
}
