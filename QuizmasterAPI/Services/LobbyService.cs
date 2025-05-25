using Quizmaster.Interfaces;
using Quizmaster.Models;
using Quizmaster.Datatypes;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Quizmaster.Services
{
    public class LobbyService : ILobbyService
    {
        private DatabaseContext _dbContext;
        private IConfiguration _configuration;

        public LobbyService(DatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<ReturnValue<Lobby>> CreateLobby(Lobby lobby)
        {
            await _dbContext.Lobbies.AddAsync(lobby);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<Lobby>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = lobby,
                Message = "Successfully created new lobby."
            };
        }

        public async Task<ReturnValue<Lobby>> DeleteLobby(int lobbyID)
        {
            Lobby? lobbyFromDatabase = await _dbContext.Lobbies.FindAsync(lobbyID);
            if (lobbyFromDatabase == null)
            {
                return new ReturnValue<Lobby>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Lobby does not exist."
                };
            }

             _dbContext.Lobbies.Remove(lobbyFromDatabase);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<Lobby>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = lobbyFromDatabase,
                Message = "Successfully deleted lobby."
            };
        }

        public async Task<ReturnValue<List<Lobby>>> GetLobbyList()
        {
            List<Lobby> lobbies = await _dbContext.Lobbies.ToListAsync();

            return new ReturnValue<List<Lobby>>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = lobbies,
                Message = "Returned list of all lobbies."
            };
        }

        public async Task<ReturnValue<Lobby>> ReadLobby(int lobbyID)
        {
            Lobby? lobby = await _dbContext.Lobbies.FindAsync(lobbyID);

            if (lobby == null)
            {
                return new ReturnValue<Lobby>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Lobby does not exist."
                };
            }

            return new ReturnValue<Lobby>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = lobby,
                Message = $"Successfully retrieved lobby {lobby.ID}"
            };
        }

        public async Task<ReturnValue<Lobby>> UpdateLobby(Lobby lobby)
        {
            Lobby? lobbyFromDatabase = await _dbContext.Lobbies.FindAsync(lobby.ID);

            if (lobbyFromDatabase == null)
            {
                return new ReturnValue<Lobby>()
                {
                    Code = HttpStatusCode.BadRequest,
                    IsError = true,
                    Message = "Lobby does not exist, update could not be executed."
                };
            }

            lobbyFromDatabase.MaxPlayers = lobby.MaxPlayers;
            lobbyFromDatabase.IsPrivate = lobby.IsPrivate;
            lobbyFromDatabase.Description = lobby.Description;
            lobbyFromDatabase.Password = lobby.Password;
            lobbyFromDatabase.QuizID = lobby.QuizID;

            _dbContext.Lobbies.Update(lobbyFromDatabase);
            await _dbContext.SaveChangesAsync();

            return new ReturnValue<Lobby>()
            {
                Code = HttpStatusCode.OK,
                IsError = false,
                Value = lobbyFromDatabase,
                Message = $"Successfully updated quiz {lobbyFromDatabase.ID}"
            };
        }
    }
}