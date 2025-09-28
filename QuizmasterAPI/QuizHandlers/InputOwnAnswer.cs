using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;
using Quizmaster.Messaging;
using Quizmaster.Models;

public class InputOwnAnswer : IQuizHandler
{
    public bool CanBuzz()
    {
        return true;
    }

    public async Task HandleAnswer(QuizQuestion question, string answer, DatabaseContext _dbContext, GameHub _hub, int lobbyID, int userID, int questionID)
    {
        BuzzInQuestionHandler handler = new BuzzInQuestionHandler();

        var correctAnswerLower = question.Answer.ToLower();
        var userAnswerLower = answer.ToLower();

        if (correctAnswerLower == userAnswerLower)
        {
            await handler.CorrectAnswer(question, answer, _dbContext, _hub, lobbyID, userID, questionID);
        }
        else
        {
            await handler.IncorrectAnswer(question, answer, _dbContext, _hub, lobbyID, userID, questionID);
        }

    }

    public async Task HandleTimeout(GameHub _hub, DatabaseContext _dbContext, int lobbyID, int questionID)
    {
        BuzzInQuestionHandler handler = new();
        await handler.HandleTimeout(_hub, _dbContext, lobbyID, questionID);
    }
}