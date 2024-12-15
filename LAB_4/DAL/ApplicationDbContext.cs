using LAB_4.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LAB_4.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Relation> Relations { get; set; }
    public DbSet<Tree> Trees { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tree>().HasKey(t => t.Id);
        modelBuilder.Entity<Person>().HasKey(p => p.Id);
        modelBuilder.Entity<Relation>().HasKey(r => r.Id);

        modelBuilder.Entity<Relation>()
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(r => r.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Relation>()
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(r => r.RelatedPersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
