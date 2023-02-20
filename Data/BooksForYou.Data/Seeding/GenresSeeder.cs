namespace BooksForYou.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;

    public class GenresSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Genres.Any())
            {
                return;
            }

            await dbContext.Genres.AddAsync(new Genre { Name = "Fantasy" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Science Fiction" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Biography" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Action & Adventure" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Mystery" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Thriller & Suspense" });
            await dbContext.Genres.AddAsync(new Genre { Name = "History" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Romance" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Short Story" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Children’s" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Art & Photography" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Travel" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Humor" });
            await dbContext.Genres.AddAsync(new Genre { Name = "Horror" });

            await dbContext.SaveChangesAsync();
        }
    }
}
