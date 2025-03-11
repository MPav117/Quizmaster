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

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterInfo newUserInfo)
        {
            var result = _authService.Register(newUserInfo);
            return result;
        }
    }
}