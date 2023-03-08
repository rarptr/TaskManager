using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Models.Data;
using TaskManager.API.Models.Services;
using TaskManager.API.Models;
using TaskManager.Common.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly TasksService _tasksService;

        public TasksController(ApplicationContext db)
        {
            _usersService = new UsersService(db);
            _tasksService = new TasksService(db);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var task = _tasksService.Get(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasksForCurrentUser()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return Unauthorized(Array.Empty<TaskModel>());
            }

            var result = await _tasksService.GetTasksForUser(user.Id).ToListAsync();
            return result == null ? NoContent() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommonModel>>> GetTasksByDesk(int deskId)
        {
            var result = await _tasksService.GetAll(deskId).ToListAsync();
            return result == null ? NoContent() : Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskModel model)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            if (model == null)
            {
                return BadRequest();
            }
            model.CreatorId = user.Id;

            bool result = _tasksService.Create(model);
            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] TaskModel model)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            if (model == null)
            {
                return BadRequest();
            }

            bool result = _tasksService.Update(id, model);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _tasksService.Delete(id);
            return result ? Ok() : NotFound();
        }
    }
}
