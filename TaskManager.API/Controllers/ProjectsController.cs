using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Models;
using TaskManager.API.Models.Data;
using TaskManager.API.Models.Services;
using TaskManager.Common.Models;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly ProjectsService _projectsService;

        public ProjectsController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
            _projectsService = new ProjectsService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<CommonModel>> Get()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user.Status == UserStatus.Admin)
            {
                return await _projectsService.GetAll().ToListAsync();
            }
            return await _projectsService.GetByUserId(user.Id);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var project = _projectsService.Get(id);
            return project == null ? NoContent() : Ok(project);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectModel projectModel)
        {
            if (projectModel == null)
            {
                return BadRequest();
            }

            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user == null || user.Status != UserStatus.Admin && user.Status != UserStatus.Editor)
            {
                return Unauthorized();
            }

            // TODO: вынести в сервис
            var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == user.Id);
            if (admin == null)
            {
                admin = new ProjectAdmin(user);
                _db.ProjectAdmins.Add(admin);
                _db.SaveChanges();
            }
            projectModel.AdminId = admin.Id;

            bool result = _projectsService.Create(projectModel);
            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectModel projectModel)
        {
            if (projectModel == null)
            {
                return BadRequest();
            }

            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user == null || user.Status != UserStatus.Admin && user.Status != UserStatus.Editor)
            {
                return Unauthorized();
            }

            bool result = _projectsService.Update(id, projectModel);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _projectsService.Delete(id);
            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}/users")]
        public IActionResult AddUsersToProject(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds == null)
            {
                return BadRequest();
            }

            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user == null || user.Status != UserStatus.Admin && user.Status != UserStatus.Editor)
            {
                return Unauthorized();
            }

            _projectsService.AddUsersToProject(id, usersIds);
            return Ok();
        }

        [HttpPatch("{id}/users/remove")]
        public IActionResult RemoveUsersFromProject(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds == null)
            {
                return BadRequest();
            }

            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user == null || user.Status != UserStatus.Admin && user.Status != UserStatus.Editor)
            {
                return Unauthorized();
            }

            _projectsService.RemoveUsersFromProject(id, usersIds);
            return Ok();
        }
    }
}
