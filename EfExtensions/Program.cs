using Context;
using FetchExtensions;
using System;
using System.Linq;

namespace EfExtensions
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BlogContext()) {

                db.Blogs.Add(new Models.Blog
                {
                    Url = "http://example.com",
                    Rating = 3
                });
                db.Blogs.Add(new Models.Blog
                {
                    Url = "http://test.com",
                    Rating = 4
                });
                db.SaveChanges();

                var query = new QueryReader<Query>().Read();
                var blogs = db.Blogs.FetchByQuery(query).ToList();

                Console.WriteLine($"Found {blogs.Count} blogs");
                foreach (var blog in blogs) {
                    Console.WriteLine($"Fetched {blog.Url} with rating {blog.Rating}");
                }
            }
        }
    }
}
