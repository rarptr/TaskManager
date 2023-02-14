using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Models;
using TaskManager.API.Models.Data;
using TaskManager.API.Models.Services;
using TaskManager.Common.Models;

namespace TaskManager.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        public UsersController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult TestApi()
        {
            return Ok($"Server is running. {DateTime.Now}");
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                var res = _usersService.Create(userModel);
                return res ? Ok() : NotFound();
            }
            return BadRequest(); // 400
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                var res = _usersService.Update(id, userModel);
                return res ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var res = _usersService.Delete(id);
            return res ? Ok() : NotFound();
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            return await _db.Users.Select(u => u.ToDto()).ToListAsync();
        }

        [HttpPost("all")]
        public async Task<IActionResult> CreateMultipleUsers([FromBody] List<UserModel> userModels)
        {
            if (userModels != null && userModels.Count > 0)
            {
                var res = _usersService.CreateMultipleUsers(userModels);
                return res ? Ok() : NotFound();
            }
            return BadRequest();
        }
    }
}
