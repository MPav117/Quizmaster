using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quizmaster.Datatypes;
using Quizmaster.Interfaces;
using Quizmaster.Models;

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

        [HttpPost("CreateQuiz")]
        public async Task<ActionResult> CreateQuiz([FromBody] Quiz newQuiz)
        {
            ReturnValue<Quiz> quiz = await _quizService.CreateQuiz(newQuiz);

            if (quiz.IsError == true)
            {
                return BadRequest(quiz.Message);
            }

            return Ok(quiz.Value);
        }

        [HttpGet("ReadQuiz/{id}")]
        public async Task<ActionResult> ReadQuiz(int id)
        {
            ReturnValue<Quiz> quiz = await _quizService.ReadQuiz(id);
            if (quiz.IsError == true)
            {
                return BadRequest(quiz.Message);
            }

            return Ok(quiz.Value);
        }

        [HttpPut("UpdateQuiz")]
        public async Task<ActionResult> UpdateQuiz(Quiz quiz)
        {
            ReturnValue<Quiz> updatedQuiz = await _quizService.UpdateQuiz(quiz);
            if (updatedQuiz.IsError == true)
            {
                return BadRequest(updatedQuiz.Message);
            }

            return Ok(updatedQuiz.Value);
        }

        [HttpDelete("DeleteQuiz")]
        public async Task<ActionResult> DeleteQuiz(int quizID)
        {
            ReturnValue<Quiz> deletedQuiz = await _quizService.DeleteQuiz(quizID);
            if (deletedQuiz.IsError == true)
            {
                return BadRequest(deletedQuiz.Message);
            }

            return Ok(deletedQuiz.Value);
        }

        [HttpPost("CreateQuizQuestion")]
        public async Task<ActionResult> CreateQuizQuestions([FromBody] List<QuizQuestion> quizQuestions)
        {
            ReturnValue<List<QuizQuestion>> createdQuizQuestions = await _quizService.CreateQuizQuestions(quizQuestions);
            return Ok(createdQuizQuestions.Value);
        }

        [HttpGet("ReadQuestionsOfQuiz/{id}")]
        public async Task<ActionResult> ReadQuestionsOfQuiz(int id)
        {
            ReturnValue<List<QuizQuestion>> questions = await _quizService.GetAllQuestionsOfQuiz(id);
            if (questions.IsError == true)
            {
                return BadRequest(questions.Message);
            }

            return Ok(questions.Value);
        }

        public async Task<ActionResult> UpdateQuizQuestions(List<QuizQuestion> quizQuestions)
        {
            ReturnValue<List<QuizQuestion>> questions = await _quizService.UpdateQuizQuestions(quizQuestions);
            if (questions.IsError == true)
            {
                return BadRequest(questions.Message);
            }

            return Ok(questions.Value);
        }

        public async Task<ActionResult> DeleteQuizQuestions(List<int> quizQuestionIDs)
        {
            ReturnValue<List<QuizQuestion>> deletedQuestions = await _quizService.DeleteQuizQuestions(quizQuestionIDs);
            if (deletedQuestions.IsError == true)
            {
                return BadRequest(deletedQuestions.Message);
            }

            return Ok(deletedQuestions.Value);
        }
    }
}