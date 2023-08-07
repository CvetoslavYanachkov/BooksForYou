namespace BooksForYou.Web.ViewModels.ContactUs
{
    using System.ComponentModel.DataAnnotations;

    public class ContactUsViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Subject { get; set; }

        [Required]
        [StringLength(3000, MinimumLength = 20)]
        public string Message { get; set; }
    }
}
