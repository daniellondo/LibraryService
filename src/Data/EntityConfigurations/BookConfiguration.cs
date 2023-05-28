namespace Data.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(entity => new { entity.Id });
            builder.Property(b => b.EditorialId).IsRequired(false);
            builder.HasOne(b => b.Editorial)
            .WithMany(e => e.Books)
            .HasForeignKey(b => b.EditorialId)
            .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
