using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;
using Quizmaster.Messaging;
using Quizmaster.Models;

public class ChooseOutOf : IQuizHandler
{
    public bool CanBuzz()
    {
        return true;
    }

    public bool ValidateQuizAnswer(QuizQuestion question, string answer)
    {
        if (answer == question.Answer)
        {
            return true;
        }

        return false;
    }

    public async Task HandleAnswer(QuizQuestion question, string answer, DatabaseContext _dbContext, GameHub _hub, int lobbyID, int userID, int questionID)
    {
        BuzzInQuestionHandler handler = new BuzzInQuestionHandler();

        if (question.Answer == answer)
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