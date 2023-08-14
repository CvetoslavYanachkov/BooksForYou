namespace BooksForYou.Services.Data.Tests.Common
{
    using BooksForYou.Data;
    using Microsoft.EntityFrameworkCore;

    public class InMemoryDatabase
    {
        public static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("TestDatabase");

            var context = new ApplicationDbContext(options.Options);

            return context;
        }
    }
}
