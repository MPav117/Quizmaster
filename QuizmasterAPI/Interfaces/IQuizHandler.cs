using Microsoft.AspNetCore.SignalR;
using Quizmaster.Datatypes;
using Quizmaster.Messaging;
using Quizmaster.Models;

namespace Quizmaster.Interfaces
{
    public interface IQuizHandler
    {
        public Task HandleAnswer(QuizQuestion question, string answer, DatabaseContext _dbContext, GameHub _hub, int lobbyID, int userID, int questionID);

        public bool CanBuzz();

        public Task HandleTimeout(GameHub _hub, DatabaseContext _dbContext, int lobbyID, int questionID);
    }
}
