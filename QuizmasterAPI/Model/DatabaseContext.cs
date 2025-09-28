using Microsoft.EntityFrameworkCore;

namespace Quizmaster.Models
{
    public class DatabaseContext : DbContext
    {   
        public DbSet<User> Users { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<QuizQuestion> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<LobbyLog> Logs { get; set; }
        public DbSet<LobbySession> Sessions { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }   
}   