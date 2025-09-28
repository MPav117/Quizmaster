using System.Security.Cryptography.X509Certificates;
using Quizmaster.Interfaces;

public class QuizHandlerFactory : IQuizHandlerFactory
{
    public IQuizHandler CreateQuizHandler(int id)
    {
        return id switch
        {
            0 => new ChooseOutOf(),
            1 => new InputOwnAnswer(),
            2 => new ClosestNumber(),
            3 => new PriceIsRight(),
            _ => new InputOwnAnswer(),
        };
    }
}