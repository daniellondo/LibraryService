namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Editorial
    {
        public Editorial()
        {
            ModifiedOn = DateTime.Now;
            Id = Guid.NewGuid();
        }
        public DateTime ModifiedOn { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public List<Book>? Books { get; set; }
    }
}
