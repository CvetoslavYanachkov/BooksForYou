namespace BooksForYou.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;

    internal class LanguagesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Languages.Any())
            {
                return;
            }

            await dbContext.Languages.AddAsync(new Language { Name = "English" });
            await dbContext.Languages.AddAsync(new Language { Name = "French" });
            await dbContext.Languages.AddAsync(new Language { Name = "German" });
            await dbContext.Languages.AddAsync(new Language { Name = "Danish" });
            await dbContext.Languages.AddAsync(new Language { Name = "Spanish" });
            await dbContext.Languages.AddAsync(new Language { Name = "Italian" });
            await dbContext.Languages.AddAsync(new Language { Name = "Greek" });
            await dbContext.Languages.AddAsync(new Language { Name = "Portuguese" });
            await dbContext.Languages.AddAsync(new Language { Name = "Finnish" });
            await dbContext.Languages.AddAsync(new Language { Name = "Swedish" });
            await dbContext.Languages.AddAsync(new Language { Name = "Czech" });
            await dbContext.Languages.AddAsync(new Language { Name = "Estonian" });
            await dbContext.Languages.AddAsync(new Language { Name = "Hungarian" });
            await dbContext.Languages.AddAsync(new Language { Name = "Polish" });
            await dbContext.Languages.AddAsync(new Language { Name = "Bulgarian" });
            await dbContext.Languages.AddAsync(new Language { Name = "Romanian" });
            await dbContext.Languages.AddAsync(new Language { Name = "Slovak" });

            await dbContext.SaveChangesAsync();
        }
    }
}
