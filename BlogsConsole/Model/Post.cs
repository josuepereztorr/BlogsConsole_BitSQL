namespace BlogsConsole
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public override string ToString()
        {
            return $"Blog: {Blog.Name}\n" +
                   $"Title: {Title}\n" +
                   $"Content: {Content}";
        }
    }
}