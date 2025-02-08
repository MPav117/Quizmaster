public namespace Quizmaster.Interfaces 
{
    public interface IQuizService
    {
        public Task<Quiz> CreateQuiz();
        public Task<Quiz> ReadQuiz();
        public Task<Quiz> UpdateQuiz();
        public Task<Quiz> DeleteQuiz();
    }
}