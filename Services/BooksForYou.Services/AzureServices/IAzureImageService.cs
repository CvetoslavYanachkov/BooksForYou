﻿namespace BooksForYou.Services.AzureServices
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IAzureImageService
    {
        Task<Uri> UploadImageToAzureAsync(IFormFile file);

        Task DeleteImageFromAzureAsync(string uriString);
    }
}
