using Quizmaster.Interfaces;
using Quizmaster.Models;
using Quizmaster.Datatypes;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Quizmaster.Services
{
    public class QuizService : IQuizService
    {
        private DatabaseContext _dbContext;
        private IConfiguration _configuration;

        public QuizService(DatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<ReturnValue<Quiz>> CreateQuiz(Quiz newQuiz)
        {
            await _dbContext.Quizzes.AddAsync(newQuiz);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<Quiz>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = newQuiz,
                Message = "Successfully created new quiz."
            };
        }

        public async Task<ReturnValue<QuizQuestion>> CreateQuizQuestion(QuizQuestion newQuizQuestion)
        {
            await _dbContext.Questions.AddAsync(newQuizQuestion);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<QuizQuestion>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = newQuizQuestion,
                Message = "Successfully created new quiz question."
            };
        }

        public async Task<ReturnValue<Quiz>> DeleteQuiz(int quizID)
        {
            Quiz? quizFromDatabase = await _dbContext.Quizzes.FindAsync(quizID);
            if (quizFromDatabase == null)
            {
                return new ReturnValue<Quiz>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = false,
                    Message = "Quiz was not found in database."
                };
            }

            _dbContext.Quizzes.Remove(quizFromDatabase);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<Quiz>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quizFromDatabase,
                Message = "Successfully deleted quiz."
            };
        }

        public async Task<ReturnValue<QuizQuestion>> DeleteQuizQuestion(int questionID)
        {
            QuizQuestion? quizQuestionFromDatabase = await _dbContext.Questions.FindAsync(questionID);

            if (quizQuestionFromDatabase == null)
            {
                return new ReturnValue<QuizQuestion>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = false,
                    Message = "Quiz was not found in database."
                };
            }

            _dbContext.Questions.Remove(quizQuestionFromDatabase);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<QuizQuestion>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quizQuestionFromDatabase,
                Message = "Successfully deleted quiz."
            };
        }

        public async Task<ReturnValue<List<QuizQuestion>>> GetAllQuestionsOfQuiz(int quizID)
        {
            List<QuizQuestion> quizQuestions = await _dbContext.Questions.Where(question => question.QuizID == quizID)
                .ToListAsync();

            return new ReturnValue<List<QuizQuestion>>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quizQuestions,
                Message = $"Successfully retrieved quiz questions for quiz {quizID}"
            };
        }

        public async Task<ReturnValue<List<Quiz>>> GetAllQuizzesCreatedByUser(int userID)
        {
            List<Quiz> quizzes = await _dbContext.Quizzes.Where(quiz => quiz.CreatorID == userID).ToListAsync();

            if (quizzes.Count == 0)
            {
                return new ReturnValue<List<Quiz>>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "User does not have any quizzes created."
                };
            }

            return new ReturnValue<List<Quiz>>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quizzes,
                Message = $"Successfully retrieved quiz questions for user {userID}"
            };
        }

        public async Task<ReturnValue<Quiz>> ReadQuiz(int quizID)
        {
            Quiz? quiz = await _dbContext.Quizzes.FindAsync(quizID);

            if (quiz == null)
            {
                return new ReturnValue<Quiz>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Quiz does not exist."
                };
            }

            return new ReturnValue<Quiz>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quiz,
                Message = $"Successfully retrieved quiz {quiz.ID}"
            };
        }

        public async Task<ReturnValue<QuizQuestion>> ReadQuizQuestion(int questionID)
        {
            QuizQuestion? quizQuestion = await _dbContext.Questions.FindAsync(questionID);

            if (quizQuestion == null)
            {
                return new ReturnValue<QuizQuestion>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Quiz question does not exist."
                };
            }

            return new ReturnValue<QuizQuestion>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quizQuestion,
                Message = $"Successfully retrieved quiz {quizQuestion.ID}"
            };
        }

        public async Task<ReturnValue<Quiz>> UpdateQuiz(Quiz updatedQuiz)
        {
            Quiz? quizFromDatabase = await _dbContext.Quizzes.FindAsync(updatedQuiz.ID);

            if (quizFromDatabase == null)
            {
                return new ReturnValue<Quiz>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Quiz does not exist, update could not be executed."
                };
            }

            quizFromDatabase.QuestionCount = updatedQuiz.QuestionCount;
            quizFromDatabase.Name = updatedQuiz.Name;
            quizFromDatabase.Description = updatedQuiz.Description;

            _dbContext.Quizzes.Update(quizFromDatabase);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<Quiz>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quizFromDatabase,
                Message = $"Successfully updated quiz {quizFromDatabase.ID}"
            };
        }

        public async Task<ReturnValue<QuizQuestion>> UpdateQuizQuestion(QuizQuestion updatedQuizQuestion)
        {
            QuizQuestion? quizQuestion = await _dbContext.Questions.FindAsync(updatedQuizQuestion.ID);

            if (quizQuestion == null)
            {
                return new ReturnValue<QuizQuestion>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Quiz question does not exist."
                };
            }

            quizQuestion.Question = updatedQuizQuestion.Question;
            quizQuestion.Answer = updatedQuizQuestion.Answer;
            quizQuestion.QuestionType = updatedQuizQuestion.QuestionType;
            quizQuestion.Image = updatedQuizQuestion.Image;

            _dbContext.Questions.Update(quizQuestion);
            await _dbContext.SaveChangesAsync();
            return new ReturnValue<QuizQuestion>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quizQuestion,
                Message = $"Successfully retrieved quiz {quizQuestion.ID}"
            };
        }

        public async Task<ReturnValue<List<QuizQuestion>>> UpdateQuizQuestions(List<QuizQuestion> questions)
        {
            if (questions.Count == 0)
            {
                return new ReturnValue<List<QuizQuestion>>()
                {
                    Code = HttpStatusCode.OK,
                    IsError = false,
                    Value = questions,
                    Message = "No changes made."
                };
            }

            QuizQuestion quizQuestion = questions[0];
            Quiz? quiz = await _dbContext.Quizzes.FindAsync(quizQuestion.QuizID);

            if (quiz == null)
            {
                return new ReturnValue<List<QuizQuestion>>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Quiz not found."
                };
            }

            List<QuizQuestion> oldQuizQuestions = await _dbContext.Questions.Where(x => x.QuizID == quiz.ID).ToListAsync();
            _dbContext.RemoveRange(oldQuizQuestions);
            await _dbContext.SaveChangesAsync();

            await _dbContext.AddRangeAsync(questions);
            await _dbContext.SaveChangesAsync();

            quiz.QuestionCount = questions.Count;
            _dbContext.Quizzes.Update(quiz);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<List<QuizQuestion>>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = questions,
                Message = "Successfully updated quiz questions."
            };
        }

        public async Task<ReturnValue<List<QuizQuestion>>> CreateQuizQuestions(List<QuizQuestion> newQuizQuestions)
        {
            if (newQuizQuestions.Count == 0)
            {
                return new ReturnValue<List<QuizQuestion>>()
                {
                    Code = HttpStatusCode.OK,
                    IsError = false,
                    Value = newQuizQuestions,
                    Message = "No changes made."
                };
            }

            QuizQuestion quizQuestion = newQuizQuestions[0];
            Quiz? quiz = await _dbContext.Quizzes.FindAsync(quizQuestion.QuizID);

            if (quiz == null)
            {
                return new ReturnValue<List<QuizQuestion>>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Quiz not found."
                };
            }

            await _dbContext.AddRangeAsync(newQuizQuestions);
            await _dbContext.SaveChangesAsync();

            quiz.QuestionCount += newQuizQuestions.Count;
            _dbContext.Quizzes.Update(quiz);
            await _dbContext.SaveChangesAsync();
            
            return new ReturnValue<List<QuizQuestion>>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = newQuizQuestions,
                Message = "Successfully created new quiz questions."
            };
        }

        public async Task<ReturnValue<List<QuizQuestion>>> DeleteQuizQuestions(List<int> questionIDs)
        {
            List<QuizQuestion> quizQuestions = await _dbContext.Questions.Where(q => questionIDs.Contains(q.ID)).ToListAsync();
            _dbContext.RemoveRange(quizQuestions);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<List<QuizQuestion>>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = quizQuestions,
                Message = "Successfully deleted quiz questions."
            };
        }

    }
}
