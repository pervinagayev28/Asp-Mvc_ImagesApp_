using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.DbContexts
{
    public class MyImageDb : DbContext
    {
        public MyImageDb(DbContextOptions<MyImageDb> options)
        : base(options) { }


        public DbSet<Image> ImagesTb { get; set; }
        public DbSet<Histories> HistoriesTb { get; set; }
        public DbSet<Favorites> FavoritesTb{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().ToTable("ImagesTb");
            modelBuilder.Entity<Image>().HasKey(j => j.Id);
            modelBuilder.Entity<Histories>().ToTable("HistoriesTb");
            modelBuilder.Entity<Histories>().HasKey(j => j.Id);
            modelBuilder.Entity<Favorites>().ToTable("FavoritesTb");
            modelBuilder.Entity<Favorites>().HasKey(j => j.Id);
        
            base.OnModelCreating(modelBuilder);
        }
  

    }
}
