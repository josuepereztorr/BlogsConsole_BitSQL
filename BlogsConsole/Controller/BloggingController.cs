using System.Linq;
using BlogsConsole.View;

namespace BlogsConsole.Controller
{
    public class BloggingController
    {
        private static BloggingContext _context;
        private static NLog.Logger _logger;
        private static Display _display;

        public BloggingController(BloggingContext context, NLog.Logger logger)
        {
            _context = context;
            _logger = logger;
            _display = new Display(_context, this, _logger);
        }

        public void AddBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            _context.SaveChanges();
        }

        public void AddPostToBlog(int blogId, Post post)
        {
            Blog blog = _context.Blogs.First(blog => blog.BlogId == blogId);
            blog.Posts.Add(post);
            _context.SaveChanges();
        }
        
        
    }
}