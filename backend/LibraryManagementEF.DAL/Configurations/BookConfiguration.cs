using LibraryManagementEF.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementEF.DAL.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(b => b.ISBN)
                .IsUnique();

            builder.Property(b => b.PublishedYear)
                .IsRequired();

            builder.Property(b => b.TotalCopies)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(b => b.AvailableCopies)
                .IsRequired()
                .HasDefaultValue(1);
        }
    }
}