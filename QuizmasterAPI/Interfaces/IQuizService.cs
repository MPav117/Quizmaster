using Quizmaster.Datatypes;
using Quizmaster.Models;

namespace Quizmaster.Interfaces 
{
    public interface IQuizService
    {
        public Task<ReturnValue<List<Quiz>>> GetAllQuizzesCreatedByUser(int userId);
        public Task<ReturnValue<Quiz>> CreateQuiz(Quiz newQuiz);
        public Task<ReturnValue<Quiz>> ReadQuiz(int quizID);
        public Task<ReturnValue<Quiz>> UpdateQuiz(Quiz updatedQuiz);
        public Task<ReturnValue<Quiz>> DeleteQuiz(int quizID);

        public Task<ReturnValue<List<QuizQuestion>>> GetAllQuestionsOfQuiz(int quizID);

        public Task<ReturnValue<QuizQuestion>> CreateQuizQuestion(QuizQuestion newQuizQuestion);
        public Task<ReturnValue<List<QuizQuestion>>> CreateQuizQuestions(List<QuizQuestion> newQuizQuestions);
        
        public Task<ReturnValue<QuizQuestion>> ReadQuizQuestion(int questionID);

        public Task<ReturnValue<QuizQuestion>> UpdateQuizQuestion(QuizQuestion updatedQuizQuestion);
        public Task<ReturnValue<List<QuizQuestion>>> UpdateQuizQuestions(List<QuizQuestion> questions);

        public Task<ReturnValue<QuizQuestion>> DeleteQuizQuestion(int questionID);
        public Task<ReturnValue<List<QuizQuestion>>> DeleteQuizQuestions(List<int> questionIDs);
    }
}