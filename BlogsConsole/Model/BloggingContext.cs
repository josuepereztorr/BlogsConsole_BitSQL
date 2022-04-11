using System.Linq;
using BlogsConsole.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlogsConsole
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        private static Display _display;
        public BloggingContext(NLog.Logger logger)
        {
            _display = new Display(this, logger);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseSqlServer(@config["ConnectionStrings:DefaultDbConnection"]);
        }
        
        public void AddBlog(Blog blog)
        {
            this.Blogs.Add(blog);
            this.SaveChanges();
        }

        public void AddPostToBlog(int blogId, Post post)
        {
            Blog blog = this.Blogs.First(blog => blog.BlogId == blogId);
            blog.Posts.Add(post);
            this.SaveChanges();
        }
    }
}