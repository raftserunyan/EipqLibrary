using EipqLibrary.Domain.Core.DomainModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EipqLibrary.Infrastructure.Data
{
    public class EipqLibraryDbContext : IdentityDbContext<AdminUser>
    {
        public EipqLibraryDbContext(DbContextOptions<EipqLibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookCreationRequest> BookCreationRequests { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<PublicRefreshToken> PublicRefreshTokens { get; set; }
        public DbSet<User> Students { get; set; }
        public DbSet<AdminRefreshToken> AdminRefreshTokens { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<BookInstance> BookInstances { get; set; }
    }
}
