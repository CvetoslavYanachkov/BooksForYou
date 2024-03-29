﻿namespace BooksForYou.Services.Data.Publishers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Services.Mapping;
    using BooksForYou.Web.ViewModels.Publishers;
    using Microsoft.EntityFrameworkCore;

    public class PublishersService : IPublishersService
    {
        private readonly IDeletableEntityRepository<Publisher> _publisherRepository;

        public PublishersService(IDeletableEntityRepository<Publisher> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public async Task<Publisher> CreatePublisherAsync(PublisherCreateViewModel model)
        {
            var publisher = new Publisher()
            {
                Name = model.Name,
                Description = model.Description,
            };

            await _publisherRepository.AddAsync(publisher);
            await _publisherRepository.SaveChangesAsync();

            return publisher;
        }

        public async Task<T> GetPublisherByIdAsync<T>(int id)
        {
            var publisher = await _publisherRepository.All().Where(p => p.Id == id).To<T>().FirstOrDefaultAsync();

            return publisher;
        }

        public async Task<T> GetPublisherForEditAsync<T>(int id)
        {
            var publisher = await _publisherRepository.All().Where(p => p.Id == id).To<T>().FirstOrDefaultAsync();

            return publisher;
        }

        public async Task<PublishersListViewModel> GetPublishersAsync(int pageNumber, int pageSize)
        {
            var publishers = await _publisherRepository.All()
    .Select(p => new PublisherInListViewModel()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
    })
    .ToListAsync();

            var result = new PublishersListViewModel()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = publishers.Count()
            };

            result.Publishers = publishers
                .OrderByDescending(x => x.Id)
                .OrderByDescending(x => x.Name)
                .Skip((pageNumber * pageSize) - pageSize)
                .Take(pageSize)
                .ToList();

            return result;
        }

        public async Task<List<string>> GetPublishersNamesAsync()
        {
            return await _publisherRepository.AllAsNoTracking()
                .Select(p => p.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Publisher>> GetPublishersToCreateAsync()
        {
            return await _publisherRepository.AllAsNoTracking().ToListAsync();
        }

        public async Task UpdatePublisherAsync(int id, PublisherEditViewModel model)
        {
            var publisher = await _publisherRepository.All().Where(p => p.Id == id).FirstOrDefaultAsync();

            publisher.Name = model.Name;
            publisher.Description = model.Description;

            await _publisherRepository.SaveChangesAsync();
        }
    }
}
