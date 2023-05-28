namespace Domain.Dtos
{
    using System.Collections.Generic;

    public class BookDto
    {
        public long ISBN { get; set; }
        public EditorialDto Editorial { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Pages { get; set; }
        public List<AuthorDto> Authors { get; set; }
    }
}
