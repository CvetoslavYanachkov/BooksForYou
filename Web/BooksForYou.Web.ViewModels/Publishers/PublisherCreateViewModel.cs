namespace BooksForYou.Web.ViewModels.Publishers
{
    using System.ComponentModel.DataAnnotations;

    using static BooksForYou.Data.Common.DataConstants.Publisher;

    public class PublisherCreateViewModel
    {
        [Required]
        [StringLength(MaxPublisherName, MinimumLength = MinPublisherName)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxPublisherDescription, MinimumLength = MinPublisherDescription)]
        public string Description { get; set; }
    }
}
