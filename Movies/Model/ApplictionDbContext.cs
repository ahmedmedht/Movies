using Microsoft.EntityFrameworkCore;

namespace Movies.Model
{
    public class ApplictionDbContext : DbContext
    {
        public ApplictionDbContext(DbContextOptions<ApplictionDbContext> options): base(options) { 
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Genre>().Property(g => g.Id).ValueGeneratedOnAdd();
        }

       public DbSet <Genre> GenreSet { get; set; } 
        public DbSet<Movie> Movies { get; set; }
    }
}
