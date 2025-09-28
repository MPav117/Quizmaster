using Quizmaster.Datatypes;
using Quizmaster.Models;

namespace Quizmaster.Interfaces
{
    public interface ILobbyService
    {
        public Task<ReturnValue<Lobby>> GetLobby(int id);
        public Task<ReturnValue<List<Lobby>>> GetLobbyList();
        public Task<ReturnValue<Lobby>> CreateLobby(Lobby lobby);
        public Task<ReturnValue<Lobby>> ReadLobby(int lobbyID);
        public Task<ReturnValue<Lobby>> UpdateLobby(Lobby lobby);
        public Task<ReturnValue<Lobby>> DeleteLobby(int lobbyID);

        public Task<ReturnValue<List<LobbySession>>> GetLobbySessions(int lobbyID);
    }
}