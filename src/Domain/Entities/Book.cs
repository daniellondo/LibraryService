namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Book
    {
        public Book()
        {
            ModifiedOn = DateTime.Now;
            Id = Guid.NewGuid();
        }
        public DateTime ModifiedOn { get; set; }
        public Guid Id { get; set; }
        public long ISBN { get; set; }
        public Guid? EditorialId { get; set; }
        public Editorial? Editorial { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Pages { get; set; }
        public List<Author> Authors { get; set; }

    }
}
