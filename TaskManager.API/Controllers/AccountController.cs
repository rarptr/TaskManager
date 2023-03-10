using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models.Data;
using TaskManager.API.Models.Services;
using TaskManager.API.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using TaskManager.Common.Models;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _userService;
        public AccountController(ApplicationContext db)
        {
            _db = db;
            _userService = new UsersService(db);
        }

        [Authorize]
        [HttpGet("info")]
        public IActionResult GetCurrentUserInfo()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Email == userName);

            return user == null ? NotFound() : Ok(user.ToDto());
        }

        [HttpPost("token")]
        public IActionResult GetToken()
        {
            var userData = _userService.GetUserLoginPassFromBasicAuth(Request);
            var login = userData.Item1;
            var pass = userData.Item2;
            // TODO: обработать ситуацию при которой данный пользователь не существует
            var identity = _userService.GetIdentity(login, pass);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE, notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPatch("update")]
        public IActionResult UpdateUser([FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest();
            }

            string userName = HttpContext.User.Identity.Name;
            User userForUpdate = _db.Users.FirstOrDefault(u => u.Email == userName);
            if (userForUpdate == null)
            {
                return NotFound();
            }

            userForUpdate.FirstName = userModel.FirstName;
            userForUpdate.LastName = userModel.LastName;
            userForUpdate.Password = userModel.Password;
            userForUpdate.Phone = userModel.Phone;
            userForUpdate.Photo = userModel.Photo;

            _db.Users.Update(userForUpdate);
            _db.SaveChanges();
            return Ok();
        }
    }
}
