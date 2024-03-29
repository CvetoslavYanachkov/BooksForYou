﻿namespace BooksForYou.Web.ViewModels.Genres
{
    using System.ComponentModel.DataAnnotations;

    public class GenreCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
