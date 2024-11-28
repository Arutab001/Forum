// Data/ApplicationDbContext.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LeagueForumAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; } // Исправлено с Comment на Comments
        public DbSet<Thread> Threads { get; set; }
        
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }

        // Новая связь с Thread
        public int ThreadId { get; set; }
        public Thread Thread { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }  // ID автора

        public virtual User Author { get; set; }  // Связь с пользователем
        public virtual Post Post { get; set; }  // Связь с постом
    }



    public class Thread
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Post> Posts { get; set; }

        public ICollection<Comment> Comments { get; set; } // Исправлено с Comment на Comments
    }
}
