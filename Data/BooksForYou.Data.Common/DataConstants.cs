namespace BooksForYou.Data.Common
{
    public class DataConstants
    {
        public class ApplicationUser
        {
            public const int MaxUserFirstname = 20;
            public const int MinUserFirstname = 3;

            public const int MaxUserLastname = 20;
            public const int MinUserLastName = 3;
        }

        public class Book
        {
            public const int MaxBookTitle = 50;
            public const int MinBookTitle = 3;

            public const int MaxBookDescription = 5000;
            public const int MinBookDescription = 20;
        }

        public class Author
        {
            public const int MaxAuthorName = 50;
            public const int MinAuthorName = 3;

            public const int MaxAuthorDescription = 5000;
            public const int MinAuthorDescription = 20;

            public const int MaxAuthorBorn = 100;
            public const int MinAuthorBorn = 5;
        }

        public class Genre
        {
            public const int MaxGenreName = 50;
            public const int MinGenreName = 3;

            public const int MaxGenreDescription = 5000;
            public const int MinGenreDescription = 20;
        }

        public class Publisher
        {
            public const int MaxPublisherName = 50;
            public const int MinPublisherName = 5;

            public const int MaxPublisherDescription = 5000;
            public const int MinPublisherDescription = 20;
        }

        public class Language
        {
            public const int MaxLanguageName = 50;
            public const int MinLanguageName = 5;
        }
    }
}
