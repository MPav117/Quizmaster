using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;
using Quizmaster.Messaging;
using Quizmaster.Models;

public class ClosestNumberQuestionHandler
{
    public async Task<bool> HandleAnswer(QuizQuestion question, string answer, DatabaseContext _dbContext, GameHub _hub, int lobbyID, int userID, int questionID)
    {
        LobbySession? answerSession = await _dbContext.Sessions.FirstOrDefaultAsync(x => x.LobbyID == lobbyID && x.UserID == userID);

        if (answerSession == null)
        {
            return false;
        }

        answerSession.Answer = answer;
        _dbContext.Sessions.Update(answerSession);
        await _dbContext.SaveChangesAsync();

        List<LobbySession> allSessions = await _dbContext.Sessions.Where(x => x.LobbyID == lobbyID).ToListAsync();

        bool allAnswered = true;

        foreach (LobbySession session in allSessions)
        {
            if (session.Answer == null)
            {
                allAnswered = false;
            }
        }

        return allAnswered;
    }

    public async Task HandleTimeout(GameHub _hub, DatabaseContext _dbContext, int lobbyID, int questionID, Func<double, double, double, bool> parseFunction)
    {
        QuizQuestion? question = await _dbContext.Questions.FindAsync(questionID);
        if (question == null)
        {
            return;
        }

        LobbyLog log = await _dbContext.Logs.FirstAsync(x => x.LobbyID == lobbyID && x.QuestionID == questionID && x.Answered == false);

        List<LobbySession> allSessions = await _dbContext.Sessions.Where(x => x.LobbyID == lobbyID).ToListAsync();

        double answerParsed = Double.Parse(question.Answer);
        double? closest = null;

        foreach (LobbySession session in allSessions)
        {
            if (closest == null)
            {
                bool tryParse = double.TryParse(session.Answer, out double tryClosest);
                if (tryParse == true)
                {
                    closest = tryClosest;
                }
            }
            else
            {
                bool tryParse = double.TryParse(session.Answer, out double tryCurrent);
                if (tryParse == true)
                {
                    if (parseFunction(answerParsed, (double)closest, tryCurrent))
                    {
                        closest = tryCurrent;
                    }
                }
            }
        }

        if (closest != null)
        {
            _dbContext.Remove(log);
            string correct = closest.ToString()!;

            foreach (LobbySession session in allSessions)
            {
                if (session.Answer == correct)
                {
                    session.Points += question.PointValue;

                    LobbyLog newLog = new LobbyLog
                    {
                        LobbyID = lobbyID,
                        QuestionID = questionID,
                        UserID = session.UserID,
                        Answered = true,
                    };

                    await _dbContext.AddAsync(newLog);
                }
                else
                {
                    session.Incorrect = true;
                }
            }

            _dbContext.Sessions.UpdateRange(allSessions);
        }
        else
        {
            log.Answered = true;
            _dbContext.Logs.Update(log);
        }
        
        await _dbContext.SaveChangesAsync();

        await _hub.Clients.Group(lobbyID.ToString()).SendAsync("onTimedOut");

        await Task.Delay(3000, CancellationToken.None);

        await _hub.SendNextQuestion(lobbyID);
    }
}