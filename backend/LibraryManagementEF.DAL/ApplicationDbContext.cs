using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEF.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BookConfiguration());
            builder.ApplyConfiguration(new BorrowRecordConfiguration());
        }
    }
}