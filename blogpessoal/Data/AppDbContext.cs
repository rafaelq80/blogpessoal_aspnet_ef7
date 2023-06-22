using blogpessoal.Models;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Postagem>().ToTable("tb_postagens");
            modelBuilder.Entity<Tema>().ToTable("tb_temas");
            modelBuilder.Entity<User>().ToTable("tb_usuarios");

            _ = modelBuilder.Entity<Postagem>()
            .HasOne(_ => _.Tema)
            .WithMany(t => t.Postagem)
            .HasForeignKey("TemaId")
            .OnDelete(DeleteBehavior.Cascade);

            _ = modelBuilder.Entity<Postagem>()
            .HasOne(_ => _.User)
            .WithMany(u => u.Postagem)
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Postagem> Postagens { get; set; } = null!;
        public DbSet<Tema> Temas { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

    }
}
