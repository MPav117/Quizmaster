using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;
using Quizmaster.Messaging;
using Quizmaster.Models;

public class BuzzInQuestionHandler
{
    public async Task CorrectAnswer(QuizQuestion question, string answer, DatabaseContext _dbContext, GameHub _hub, int lobbyID, int userID, int questionID)
    {
        _hub.StopTimer(lobbyID);

        LobbySession? session = await _dbContext.Sessions.FirstOrDefaultAsync(x => x.LobbyID == lobbyID && x.UserID == userID);

        if (session == null)
        {
            Console.WriteLine("CorrectAnswer - Session not found!");
            return;
        }

        session.Points += question.PointValue;
        session.Answer = answer;

        _dbContext.Update(session);

        LobbyLog? log = await _dbContext.Logs.FirstOrDefaultAsync(x => x.LobbyID == lobbyID && x.QuestionID == questionID && x.Answered == false);

        if (log == null)
        {
            Console.WriteLine("CorrectAnswer - Log not found!");
            Console.WriteLine("Question: " + questionID.ToString());
        }
        else
        {
            log.UserID = userID;
            log.Answered = true;
            _dbContext.Logs.Update(log);
            Console.WriteLine("log!");
        }

        List<LobbySession> allSessions = await _dbContext.Sessions.Where(x => x.LobbyID == lobbyID).ToListAsync();
        foreach (LobbySession aSession in allSessions)
        {
            aSession.Incorrect = false;
        }

        _dbContext.Sessions.UpdateRange(allSessions);

        await _dbContext.SaveChangesAsync();

        await _hub.Clients.Group(session.LobbyID.ToString()).SendAsync("onCorrectAnswerRecieved", session.UserID);

        await Task.Delay(4000);

        await _hub.SendNextQuestion(lobbyID);
    }

    public async Task IncorrectAnswer(QuizQuestion question, string answer, DatabaseContext _dbContext, GameHub _hub, int lobbyID, int userID, int questionID)
    {
        LobbySession? session = await _dbContext.Sessions.FirstOrDefaultAsync(x => x.LobbyID == lobbyID && x.UserID == userID);

        if (session == null)
        {
            Console.WriteLine("IncorrectAnswer - Session not found!");
            return;
        }

        session.Points -= question.PointValue;
        session.Incorrect = true;
        session.Answer = answer;

        _dbContext.Update(session);
        await _dbContext.SaveChangesAsync();

        await _hub.Clients.Group(session.LobbyID.ToString()).SendAsync("onIncorrectAnswerRecieved", session.UserID);

        List<LobbySession> allSessions = await _dbContext.Sessions.Where(x => x.LobbyID == lobbyID).ToListAsync();

        bool everyoneIsWrong = true;

        foreach (LobbySession checkedSession in allSessions)
        {
            if (checkedSession.Incorrect == false)
            {
                everyoneIsWrong = false;
            }
        }

        if (everyoneIsWrong == true)
        {
            _hub.StopTimer(lobbyID);

            LobbyLog? log = await _dbContext.Logs.FirstOrDefaultAsync(x => x.LobbyID == lobbyID && x.QuestionID == questionID && x.Answered == false);
            if (log == null)
            {
                Console.WriteLine("CorrectAnswer - Log not found!");
            }
            else
            {
                log.UserID = userID;
                _dbContext.Update(log);
            }

            await _dbContext.SaveChangesAsync();

            await _hub.Clients.Group(session.LobbyID.ToString()).SendAsync("onTimedOut");

            await Task.Delay(4000, CancellationToken.None);

            await _hub.SendNextQuestion(lobbyID);
        }
    }

    public async Task HandleTimeout(GameHub _hub, DatabaseContext _dbContext, int lobbyID, int questionID)
    {
        _hub.StopTimer(lobbyID);

        await _hub.Clients.Group(lobbyID.ToString()).SendAsync("onTimedOut");

        Console.WriteLine("Sent on time out:" + lobbyID.ToString());

        LobbyLog log = await _dbContext.Logs.FirstAsync(x => x.LobbyID == lobbyID && x.QuestionID == questionID && x.Answered == false);
        LobbySession? session = await _dbContext.Sessions.FirstOrDefaultAsync(x => x.LobbyID == lobbyID);

        if (log == null || session == null)
        {
            Console.WriteLine("CorrectAnswer - Log not found!");
        }

        else
        {
            log.Answered = true;
            _dbContext.Update(log);
        }

        await _dbContext.SaveChangesAsync();
        
        await Task.Delay(3000, CancellationToken.None);

        await _hub.SendNextQuestion(lobbyID);
    }
}
