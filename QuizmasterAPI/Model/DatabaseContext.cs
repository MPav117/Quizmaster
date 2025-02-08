using Microsoft.EntityFrameworkCore;

namespace Quizmaster.Model
{
    public class DatabaseContext : DbContext
    {   
        public DbSet<User> Users { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }

        public DatabaseContext(DbContextOptions<DBContext> options) : base(options) {}
    }   
}   