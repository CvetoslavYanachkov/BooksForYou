namespace BooksForYou.Web.ViewModels.Votes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PostVoteViewModel
    {
        public int BookId { get; set; }

        [Range(1, 5)]
        public byte Value { get; set; }
    }
}
