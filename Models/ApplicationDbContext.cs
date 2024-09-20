using Microsoft.EntityFrameworkCore;

using TodoApi.Models.Todo;
using TodoApi.Models.User;

/// <summary>
/// This class allows coordination with the Entity Framework.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<UserItem> UserItems { get; set; } = null!;

    /// <summary>
    /// This class facilitates the 1-to-many relationship between Users and their Todos.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserItem>()
            .HasMany(u => u.Todos)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<UserItem>()
            .HasMany(u => u.Todos)
            .WithOne(t => t.User)
            .OnDelete(DeleteBehavior.Cascade);
    }
}