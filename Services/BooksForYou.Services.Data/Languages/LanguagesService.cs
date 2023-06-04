namespace BooksForYou.Services.Data.Languages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class LanguagesService : ILanguagesService
    {
        private readonly IDeletableEntityRepository<Language> _languageRepository;

        public LanguagesService(IDeletableEntityRepository<Language> languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<IEnumerable<Language>> GetLanguagesToCreateAsync()
        {
            return await _languageRepository.AllAsNoTracking().ToListAsync();
        }
    }
}
