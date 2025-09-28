using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;
using Quizmaster.Datatypes;

namespace Quizmaster.Controllers
{
    [ApiController]
    [Route("API/Auth")]

    public class AuthController : Controller 
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var result = await _authService.GetUser(id);

            if (result.IsError == false)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginInfo UserInfo)
        {
            var result = await _authService.Login(UserInfo);
            if (result.IsError == false)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterInfo newUserInfo)
        {
            var result = await _authService.Register(newUserInfo);
            if (result.IsError == false)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}