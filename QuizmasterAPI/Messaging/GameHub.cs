using System.Security.Cryptography;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;
using Quizmaster.Models;

namespace Quizmaster.Messaging
{
    public class GameHub : Hub
    {
        public DatabaseContext _dbContext;
        private static readonly Dictionary<string, CancellationTokenSource> _cancellationTokens = new();

        public GameHub(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();
            if(context != null)
            {
                await base.OnConnectedAsync();
            }
            else
            {
                Context.Abort();
            }
        }

        public async Task AddToLobby(int lobbyID, int userID)
        {
            // lobby needs to exist
            Lobby? lobby = await _dbContext.Lobbies.FindAsync(lobbyID);
            if (lobby == null)
            {
                return;
            }

            int playerCount = _dbContext.Sessions.Count(e => e.LobbyID == lobbyID);
            int readyCount = _dbContext.Sessions.Count(e => e.LobbyID == lobbyID && e.Ready == true);

            // can't join if lobby is full!
            if (lobby.MaxPlayers <= playerCount + 1)
            {
                return;
            }

            // user needs to exist
            User? user = await _dbContext.Users.FindAsync(userID);
            if (user == null)
            {
                return;
            }

            // if user is in
            LobbySession? session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.UserID == userID && s.LobbyID == lobbyID);
            if (session != null)
            {
                _dbContext.Sessions.Remove(session);
                await _dbContext.SaveChangesAsync();
                return;
            }

            LobbySession newSession = new LobbySession
            {
                LobbyID = lobbyID,
                UserID = userID,
                ConnectionId = Context.ConnectionId,
                Points = 0,
                Ready = false,
                Incorrect = false,
            };

            await _dbContext.Sessions.AddAsync(newSession);
            lobby.CurrentPlayers = playerCount + 1;
            lobby.ReadyPlayers = readyCount;

            _dbContext.Lobbies.Update(lobby);
            await _dbContext.SaveChangesAsync();
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyID.ToString());

            await Clients.Group(lobbyID.ToString()).SendAsync("onUserJoinLeaveLobby");
        }

        public async Task RemoveFromLobby(int lobbyID, int userID)
        {
            // lobby needs to exist
            Lobby? lobby = await _dbContext.Lobbies.FindAsync(lobbyID);
            if (lobby == null)
            {
                return;
            }

            // user needs to exist
            User? user = await _dbContext.Users.FindAsync(userID);
            if (user == null)
            {
                return;
            }

            // user needs to be in session
            LobbySession? session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.UserID == userID && s.LobbyID == lobbyID);
            if (session == null)
            {
                return;
            }

            _dbContext.Sessions.Remove(session);
            await _dbContext.SaveChangesAsync();

            // TODO - Notify group of leaving here!
        }

        public async Task Ready()
        {
            LobbySession? session = await _dbContext.Sessions.Include(e => e.Lobby).FirstOrDefaultAsync(x => x.ConnectionId == Context.ConnectionId);

            Console.WriteLine("Entered!");

            if (session == null)
            {
                return;
            }

            if (session.Ready == true)
            {
                return;
            }

            Console.WriteLine("Passed checks!");

            session.Ready = true;
            session.Lobby.ReadyPlayers += 1;

            Console.WriteLine("Edited session!");

            _dbContext.Sessions.Update(session);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine("Saved session!");

            await Clients.Group(session.Lobby.ID.ToString()).SendAsync("onUserReadyUnready");

            Console.WriteLine("Notified group!");
        }

        public async Task Unready()
        {
            LobbySession? session = await _dbContext.Sessions.Include(e => e.Lobby).FirstOrDefaultAsync(x => x.ConnectionId == Context.ConnectionId);
            if (session == null)
            {
                return;
            }

            if (session.Ready == false)
            {
                return;
            }

            session.Ready = false;
            session.Lobby.ReadyPlayers -= 1;

            _dbContext.Sessions.Update(session);
            await _dbContext.SaveChangesAsync();

            await Clients.Group(session.Lobby.ID.ToString()).SendAsync("onUserReadyUnready");
            
            // TODO -
        }

        public async Task NotifyOfLobbyUpdate(int lobbyID)
        {
            await Clients.Group(lobbyID.ToString()).SendAsync("onLobbyUpdated");   
        }

        // For host only
        public async Task StartQuiz(int lobbyID, int quizID)
        {
            Console.WriteLine("Entered start quiz!");

            Lobby? lobby = await _dbContext.Lobbies.FindAsync(lobbyID);

            if (lobby == null)
            {
                return;
            }
            
            if (lobby.ReadyPlayers < lobby.CurrentPlayers)
            {
                // notify?
                return;
            }

            if (lobby.Status != 0)
            {
                return;
            }

            List<QuizQuestion> questions = await _dbContext.Questions.Where(e => e.QuizID == quizID).ToListAsync();
            Console.WriteLine(questions.Count);

            List<LobbyLog> potentialPreviousLobbyLogsInTheLobby = await _dbContext.Logs.Where(x => x.LobbyID == lobbyID).ToListAsync();

            _dbContext.Logs.RemoveRange(potentialPreviousLobbyLogsInTheLobby);

            List<LobbyLog> lobbyLogs = new List<LobbyLog>();

            foreach (QuizQuestion question in questions)
            {
                LobbyLog newLog = new LobbyLog
                {
                    QuestionID = question.ID,
                    LobbyID = lobbyID,
                    UserID = null,
                    Answered = false
                };
                await _dbContext.Logs.AddAsync(newLog);
            }

            List<LobbySession> sessions = await _dbContext.Sessions.Where(x => x.LobbyID == lobbyID).ToListAsync();

            foreach (LobbySession session in sessions)
            {
                session.Points = 0;
                session.Incorrect = false;
                session.Answer = null;
            }

            _dbContext.Sessions.UpdateRange(sessions);

            await _dbContext.SaveChangesAsync();

            // switch view from lobby display to quiz display
            await Clients.Group(lobbyID.ToString()).SendAsync("onQuizStart");

            await SendNextQuestion(lobbyID);
        }

        public async Task SendNextQuestion(int lobbyID)
        {
            const int timerLength = 180;
            _cancellationTokens.Remove(lobbyID.ToString());

            List<LobbySession> allSessions = await _dbContext.Sessions.Include(x => x.User).Where(x => x.LobbyID == lobbyID).ToListAsync();
            foreach (LobbySession checkedSession in allSessions)
            {
                checkedSession.Incorrect = false;
                checkedSession.Answer = null;
            }

            _dbContext.Sessions.UpdateRange(allSessions);

            await _dbContext.SaveChangesAsync();

            LobbyLog? log = await _dbContext.Logs.FirstOrDefaultAsync(e => e.LobbyID == lobbyID && e.UserID == null);
            if (log == null)
            {
                await EndQuiz(lobbyID);
            }
            else
            {
                await Clients.Group(lobbyID.ToString()).SendAsync("onGetNextQuestion", log.QuestionID);
                await Clients.Group(lobbyID.ToString()).SendAsync("onTimeoutRecieved", timerLength);

                var cancellationTokenSourceOfLobby = new CancellationTokenSource();

                _cancellationTokens.Add(lobbyID.ToString(), cancellationTokenSourceOfLobby);
                await TimeOut(lobbyID, log.QuestionID, timerLength, cancellationTokenSourceOfLobby.Token);
            }
        }

        public async Task EndQuiz(int lobbyID)
        {
            List<LobbySession> allSessions = await _dbContext.Sessions.Include(x => x.User).Where(x => x.LobbyID == lobbyID).ToListAsync();
            Lobby? lobby = await _dbContext.Lobbies.FindAsync(lobbyID);

            foreach (LobbySession checkedSession in allSessions)
            {
                checkedSession.Incorrect = false;
                checkedSession.Answer = null;
                checkedSession.Ready = false;
                checkedSession.User.Experience += RandomNumberGenerator.GetInt32(80, 100);
                if (checkedSession.User.Experience >= checkedSession.User.Level * 200)
                {
                    checkedSession.User.Experience -= checkedSession.User.Level * 200;
                    checkedSession.User.Level += 1;
                }
            }

            if (lobby != null)
            {
                lobby.ReadyPlayers = 0;
                _dbContext.Lobbies.Update(lobby);
            }

            _dbContext.Sessions.UpdateRange(allSessions);

            await _dbContext.SaveChangesAsync();

            await Clients.Group(lobbyID.ToString()).SendAsync("onQuizEnded");
        }

        public void StopTimer(int lobbyID)
        {
            if (_cancellationTokens.TryGetValue(lobbyID.ToString(), out CancellationTokenSource? tokenSource))
            {
                tokenSource.Cancel();
            }
        }

        // For host only
        public async Task ResumeQuiz(int lobbyID)
        {
            await Clients.Group(lobbyID.ToString()).SendAsync("onLobbyResumed");
            await SendNextQuestion(lobbyID);
        }

        // For host only
        public async Task StopQuiz(int lobbyID)
        {
            StopTimer(lobbyID);
            await Clients.Group(lobbyID.ToString()).SendAsync("onLobbyStopped");
        }

        public async Task BuzzIn(int lobbyID, int questionID, int userID)
        {
            QuizQuestion? question = await _dbContext.Questions.FindAsync(questionID);
            
            if (question == null)
            {
                return;
            }
  
            IQuizHandlerFactory quizHandlerFactory = new QuizHandlerFactory();
            IQuizHandler quizHandler = quizHandlerFactory.CreateQuizHandler(question.QuestionType);

            if (quizHandler.CanBuzz())
            {
                await Clients.Group(lobbyID.ToString()).SendAsync("onBuzzRecieved", userID);
            }
        }

        public async Task AnswerQuestion(int lobbyID, int questionID, int userID, string answer)
        {
            QuizQuestion? question = await _dbContext.Questions.FindAsync(questionID);
            if (question == null)
            {
                return;
            }

            IQuizHandlerFactory quizHandlerFactory = new QuizHandlerFactory();
            IQuizHandler quizHandler = quizHandlerFactory.CreateQuizHandler(question.QuestionType);

            await quizHandler.HandleAnswer(question, answer, _dbContext, this, lobbyID, userID, questionID);
        }

        public async Task TimeOut(int lobbyID, int questionID, int seconds, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(seconds * 1000, cancellationToken);
                Console.WriteLine("Timed Out!");
                Console.WriteLine("Cancel requested:" + cancellationToken.IsCancellationRequested.ToString());

                if (cancellationToken.IsCancellationRequested == false)
                {
                    QuizQuestion? quizQuestion = await _dbContext.Questions.FindAsync(questionID);
                    if (quizQuestion == null)
                    {
                        Console.WriteLine("Timeout - Quiz Question does not exist!");
                        return;
                    }

                    _cancellationTokens.Remove(lobbyID.ToString());
                    Console.WriteLine("Token removed!");

                    IQuizHandlerFactory quizHandlerFactory = new QuizHandlerFactory();
                    IQuizHandler quizHandler = quizHandlerFactory.CreateQuizHandler(quizQuestion.QuestionType);

                    await quizHandler.HandleTimeout(this, _dbContext, lobbyID, questionID);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine(Context.ConnectionId);
            LobbySession? session = await _dbContext.Sessions.Include(e => e.Lobby).FirstOrDefaultAsync(s => s.ConnectionId == Context.ConnectionId);

            if (session != null && session.Lobby.Status == 0)
            {
                session.Lobby.CurrentPlayers = session.Lobby.CurrentPlayers - 1;

                if (session.Lobby.CurrentPlayers <= 0)
                {
                    Console.WriteLine("Removed lobby! Yes!");
                    _dbContext.Lobbies.Remove(session.Lobby);
                }
                else
                {
                    if (session.Ready == true)
                    {
                        session.Lobby.ReadyPlayers -= 1;
                        _dbContext.Lobbies.Update(session.Lobby);
                    }
                }
    
                _dbContext.Sessions.Remove(session);

                await _dbContext.SaveChangesAsync();

                await Clients.Group(session.LobbyID.ToString()).SendAsync("onUserJoinLeaveLobby");
            }
        }
    }
}