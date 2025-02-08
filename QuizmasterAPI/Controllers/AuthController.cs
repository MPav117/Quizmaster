using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}