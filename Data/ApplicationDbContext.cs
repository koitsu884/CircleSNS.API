using CircleSNS.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CircleSNS.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options){

        }

        public DbSet<Member> Members { get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Member>()
                .HasOne(m=>m.Identity)
                .WithOne()
                .HasForeignKey<Member>(m => m.IdentityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}