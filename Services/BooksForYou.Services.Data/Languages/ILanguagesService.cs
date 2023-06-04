namespace BooksForYou.Services.Data.Languages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Models;

    public interface ILanguagesService
    {
        Task<IEnumerable<Language>> GetLanguagesToCreateAsync();
    }
}
