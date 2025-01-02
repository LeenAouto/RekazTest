using Microsoft.EntityFrameworkCore;
using RekazTest.Models;
using System.Net.Sockets;

namespace RekazTest.DatabaseAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Setting Guid to be auto-gnerated 
            modelBuilder.Entity<Blob>()
                .Property(i => i.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<BlobMetadata>()
                .Property(i => i.Id)
                .HasDefaultValueSql("NEWID()");
        }
        public DbSet<Blob> Blobs { get; set; }
        public DbSet<BlobMetadata> BlobsMetadata { get; set; }

    }
}
