using BiblioTechA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BiblioTechA.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUserBook>().HasKey(ab => new { ab.ApplicationUserId, ab.BookId });
            builder.Entity<ApplicationUserBookHistory>().HasKey(abh => new { abh.ApplicationUserId, abh.BookReservationHistoryId });

            builder.Entity<Book>()
                .HasMany(b => b.BooksReservationHistory)
                .WithOne(br => br.Book)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUserBook>()
                .HasOne(sc => sc.ApplicationUser)
                .WithMany(s => s.UserBooks)
                .HasForeignKey(sc => sc.ApplicationUserId);

            builder.Entity<ApplicationUserBook>()
                .HasOne(sc => sc.Book)
                .WithMany(s => s.UserBooks)
                .HasForeignKey(sc => sc.BookId);

            builder.Entity<ApplicationUserBookHistory>()
                .HasOne(sc => sc.ApplicationUser)
                .WithMany(s => s.UsersBookHistory)
                .HasForeignKey(sc => sc.ApplicationUserId);

            builder.Entity<ApplicationUserBookHistory>()
                .HasOne(sc => sc.BookReservationHistory)
                .WithMany(s => s.UsersBookHistory)
                .HasForeignKey(sc => sc.BookReservationHistoryId);

        }

        public DbSet<Book> Book { get; set; }
        public DbSet<BookReservationHistory> BookReservationHistory { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<ApplicationUserBook> UserBooks { get; set; }

        public DbSet<ApplicationUserBookHistory> UserBooksHistory { get; set; }
    }
}
