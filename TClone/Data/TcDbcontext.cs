
using Microsoft.EntityFrameworkCore;
using TClone.Models;

namespace TClone.Data
{
    public class TcDbcontext:DbContext
    {
        public TcDbcontext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Like> Likes { get; set; }

       
    }
}
