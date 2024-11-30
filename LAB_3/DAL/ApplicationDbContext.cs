using LAB_3.Models;
using Microsoft.EntityFrameworkCore;

namespace LAB_3.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Batch> Batches { get; set; }  

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
                .HasKey(s => s.StoreId);

            modelBuilder.Entity<Product>()
                .HasKey(p => p.Name); 

            modelBuilder.Entity<Batch>()
                .HasKey(b => new { b.StoreId, b.ProductName }); 

            modelBuilder.Entity<Batch>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(b => b.ProductName); 

            modelBuilder.Entity<Batch>()
                .HasOne<Store>()
                .WithMany()
                .HasForeignKey(b => b.StoreId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
