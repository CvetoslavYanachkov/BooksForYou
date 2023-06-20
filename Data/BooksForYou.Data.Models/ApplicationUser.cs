namespace BooksForYou.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BooksForYou.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;

    using static BooksForYou.Data.Common.DataConstants.ApplicationUser;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [StringLength(MaxUserFirstname)]
        public string FirstName { get; set; }

        [StringLength(MaxUserLastname)]
        public string LastName { get; set; }

        [StringLength(MaxUserWebsite)]
        public string Website { get; set; }

        [StringLength(MaxUserDescription)]
        public string Description { get; set; }

        [StringLength(MaxUserBorn)]
        public string Born { get; set; }

        //public int GenreId { get; set; }

        //[ForeignKey(nameof(GenreId))]
        //public Genre Genre { get; set; }

        public string ImageUrl { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Books { get; set; }

        public virtual ICollection<Book> Vots { get; set; }
    }
}
