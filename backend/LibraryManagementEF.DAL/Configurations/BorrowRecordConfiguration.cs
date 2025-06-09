using LibraryManagementEF.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementEF.DAL.Configurations
{
    public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            builder.ToTable("BorrowRecords");

            builder.HasKey(br => br.Id);

            builder.Property(br => br.BorrowDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(br => br.DueDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(br => br.ReturnDate)
                .HasColumnType("datetime2");

            builder.Property(br => br.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Borrowed");

            builder.HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(br => br.User)
                .WithMany()
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}