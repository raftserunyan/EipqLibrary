using EipqLibrary.Domain.Core.DomainModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EipqLibrary.Infrastructure.Data
{
    public class EipqLibraryDbContext : IdentityDbContext<User>
    {
        public EipqLibraryDbContext(DbContextOptions<EipqLibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PublicRefreshToken> PublicRefreshTokens { get; set; }
    }
}
