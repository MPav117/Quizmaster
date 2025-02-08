using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Quizmaster.Controllers
{
    [ApiController]
    [Route("API/Lobby")]

    public class QuizController : Controller 
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }
    }
}