using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ServiceOrder> ServiceOrders { get; set; }
    public DbSet<People> People { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Dispatcher> Dispatchers { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Ensure Email Uniqueness
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Seed Default Roles
        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            },
        };
        builder.Entity<IdentityRole>().HasData(roles);

        // Configure Relationships and Delete Behaviors

        // Relationship between Article and User
        builder.Entity<Article>()
            .HasOne(a => a.CreatedBy)
            .WithMany(u => u.Articles)
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // Relationship between ServiceOrder and Company
        builder.Entity<ServiceOrder>()
            .HasOne(so => so.Company)
            .WithMany(c => c.ServiceOrders)
            .HasForeignKey(so => so.CompanyId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // One-to-Many relationship between ServiceOrder and User
        builder.Entity<ServiceOrder>()
            .HasOne(so => so.User) // A ServiceOrder has one User
            .WithMany(u => u.ServiceOrders) // A User has many ServiceOrders
            .HasForeignKey(so => so.UserId); // Foreign key in ServiceOrder table


    }
}