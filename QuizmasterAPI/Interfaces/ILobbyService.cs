public namespace Quizmaster.Interfaces
{
    public interface ILobbyService 
    {
        public Task<List<Lobby>> GetLobbyList();
        public Task<Lobby> CreateLobby();
        public Task<Lobby> ReadLobby();
        public Task<Lobby> UpdateLobby();
        public Task<Lobby> DeleteLobby();
    }
}