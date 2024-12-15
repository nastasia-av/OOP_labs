using LAB_4.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LAB_4.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<Tree> Trees { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(p => p.Id);
            modelBuilder.Entity<Relation>().HasKey(r => r.Id);

            modelBuilder.Entity<Relation>()
                .HasOne<Person>()
                .WithMany(p => p.Relations)
                .HasForeignKey(r => r.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Relation>()
                .HasIndex(r => new { r.PersonId, r.RelatedPersonId, r.RelationType })
                .IsUnique();

            modelBuilder.Entity<Tree>()
                .HasKey(t => t.Id); 
            modelBuilder.Entity<Tree>()
                .HasMany(t => t.Persons)
                .WithMany(); 
        }
    }
}
