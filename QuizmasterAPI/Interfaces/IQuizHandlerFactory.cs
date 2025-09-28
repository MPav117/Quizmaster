namespace Quizmaster.Interfaces
{
    public interface IQuizHandlerFactory
    {
        public IQuizHandler CreateQuizHandler(int id);
    }
}