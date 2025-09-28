using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;
using Quizmaster.Messaging;
using Quizmaster.Models;

public class PriceIsRight : IQuizHandler
{
    public bool CanBuzz()
    {
        return false;
    }

    public async Task HandleAnswer(QuizQuestion question, string answer, DatabaseContext _dbContext, GameHub _hub, int lobbyID, int userID, int questionID)
    {
        ClosestNumberQuestionHandler handler = new ClosestNumberQuestionHandler();

        bool allAnswered = await handler.HandleAnswer(question, answer, _dbContext, _hub, lobbyID, userID, questionID);

        if (allAnswered == true)
        {
            _hub.StopTimer(lobbyID);
            await HandleTimeout(_hub, _dbContext, lobbyID, questionID);
        }
    }

    public bool ParseFunction(double answerParsed, double closest, double tryCurrent)
    {
        if (Double.Abs((int)(answerParsed - closest)) > Double.Abs(answerParsed - tryCurrent) && (tryCurrent <= answerParsed))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task HandleTimeout(GameHub _hub, DatabaseContext _dbContext, int lobbyID, int questionID)
    {
        ClosestNumberQuestionHandler handler = new ClosestNumberQuestionHandler();
        await handler.HandleTimeout(_hub, _dbContext, lobbyID, questionID, ParseFunction);
    }
}