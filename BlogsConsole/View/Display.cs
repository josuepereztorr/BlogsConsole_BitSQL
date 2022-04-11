using System;
using System.Linq;
using BlogsConsole.Controller;

namespace BlogsConsole.View
{
    public class Display
    {
        private static BloggingContext _context;
        private static BloggingController _controller;
        private static NLog.Logger _logger;
        
        public Blog Blog { get; set; }
        public Post Post { get; set; }
        
        private string _input;
        
        private const string AllBlog = "ALLBLOGS";
        private const string AllPost = "ALLPOSTS";
        private const string AllPostsFromBlog = "ALLPOSTSFROMBLOG";
        
        public Display(BloggingContext context, BloggingController controller, NLog.Logger logger)
        {
            _context = context;
            _controller = controller;
            _logger = logger;
            Blog = new Blog();
            Post = new Post();

            do
            {
                ShowMenu();
                
                _input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(_input) || !new string[] {"1","2","3","4"}.Contains(_input))
                {
                    _logger.Info("Invalid Selection");
                    continue;
                }

                switch (_input)
                {
                    case "1":
                        ShowAllBlogs();
                        break;
                    case "2":
                        ShowAddBlog();
                        break;
                    case "3":
                        ShowCreatePost();
                        break;
                    case "4" :
                        ShowPosts();
                        break;
                }

            } while (_input != null && !_input.Equals("q"));
        }
        
        private void ShowMenu()
        {
            Console.WriteLine("Enter your selection:");
            Console.WriteLine("1) Display all blogs");
            Console.WriteLine("2) Add Blog");
            Console.WriteLine("3) Create Post");
            Console.WriteLine("4) Display Posts");
            Console.WriteLine("Enter q to quit");
        }
        
        private void ShowAllBlogs()
        {
            _logger.Info("Option '1' selected");
            
            Console.WriteLine($"{_context.Blogs.Count()} Blogs returned");
            ShowListOfEntitiesByType(AllBlog);
            Console.WriteLine();
        }
        
        private void ShowAddBlog()
        {
            _logger.Info("Option '2' selected");
            Console.WriteLine("Enter a name for a new Blog: ");
            _input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(_input))
            {
                Blog.Name = _input;
                _controller.AddBlog(Blog);
                _logger.Info($"Blog added - '{Blog.Name}'");
            }
            else
            {
                _logger.Info("Blog name cannot be null");
            }
        }
        
        private void ShowCreatePost()
        {

            if (_context.Blogs.Count() > 0)
            {
                Console.Clear();
                _logger.Info("Option '3' selected");
                Console.WriteLine("Select the Blog you would like to post to: ");
                ShowListOfEntitiesByType(AllBlog, true);
                
                _input = Console.ReadLine();
                
                if (int.TryParse(_input, out int blogId))
                {
                    if (_context.Blogs.Any(blog => blog.BlogId.Equals(blogId)))
                    {
                        Console.WriteLine("Enter the Post title: ");
                        
                        _input = Console.ReadLine();
                        
                        if (!string.IsNullOrWhiteSpace(_input))
                        {
                            Post.Title = _input;
                            
                            Console.WriteLine("Enter the Post content"); 
                            _input = Console.ReadLine();

                            Post.Content = _input;
                            Post.BlogId = blogId;
                            
                            Blog blog = _context.Blogs.First(blog => blog.BlogId == blogId);
                            Post.Blog = blog;
                            
                            _controller.AddPostToBlog(blogId, Post);
                            
                            _logger.Info($"Post added - '{Post.Title}'");
                            Console.WriteLine();
                        }
                        else
                        {
                            _logger.Info("Post title cannot be null");
                        }
                    }
                    else
                    {
                        _logger.Info("There are no Blogs saved with that id");
                    }
                }
                else
                {
                    _logger.Info("Invalid Blog Id");
                }
            }
            else
            {
                _logger.Info("No Blogs found");
                Console.Clear();
                Console.WriteLine("Cannot create post, there must be at least one blog created to create a new post");
                Console.WriteLine();
                Console.WriteLine("Press enter to return to the main menu...");
                Console.ReadLine();
            }
        }
        
        private void ShowPosts()
        {
            Console.Clear();
            _logger.Info("Option '4' selected");
            Console.WriteLine("Select the Blog's posts to display: ");
            Console.WriteLine("0) Posts from all blogs");
            
            ShowListOfEntitiesByType(AllPostsFromBlog, true);

            _input = Console.ReadLine();
            
            if (int.TryParse(_input, out int blogId))
            {
                if (blogId.Equals(0))
                {
                    ShowListOfEntitiesByType(AllPost);
                }
                else
                {
                    if (_context.Blogs.Any(blog => blog.BlogId == blogId))
                    {
                        ShowListOfEntitiesByType(AllPost, id:blogId);
                    }
                    else
                    {
                        _logger.Info("There are no Blogs saved with that id");
                    }
                }
            }
            else
            {
                _logger.Info("Invalid Blog Id");
            }

        }
        
        private void ShowListOfEntitiesByType(string entityType, bool showId = false, int id = 0)
        {
            switch (entityType)
            {
                case AllBlog:
                {
                    foreach (var blog in _context.Blogs)
                    {
                        Console.WriteLine(showId ? $"{blog.BlogId}) {blog.Name}" : blog.Name);
                    }
                    
                    break;
                }
                case AllPost:
                {

                    IQueryable<Post> selectAll = _context.Posts.AsQueryable();
                    IQueryable<Post> queryById = _context.Posts.Where(post => post.BlogId == id);

                    if (id != 0)
                    {
                        Console.WriteLine($"{queryById.Count()} item(s) returned");
                        Console.WriteLine();
                        foreach (var post in queryById)
                        {
                            Console.WriteLine(post.ToString());
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{selectAll.Count()} item(s) returned");
                        Console.WriteLine();
                        foreach (var post in selectAll)
                        {
                            Console.WriteLine(post.ToString());
                            Console.WriteLine();
                        }
                    }

                    break;
                }
                case AllPostsFromBlog:
                {
                    foreach (var blog in _context.Blogs)
                    {
                        Console.WriteLine($"{blog.BlogId}) Post from '{blog.Name}'");;
                    }

                    break;
                }
            }
        }
    }
}