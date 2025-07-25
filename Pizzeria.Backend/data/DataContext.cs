using Microsoft.EntityFrameworkCore;
using Pizzeria.Shared.Entites;

namespace Pizzeria.Backend.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasIndex(usuario => usuario.DNI).IsUnique();
            modelBuilder.Entity<Producto>().HasIndex(producto => producto.Nombre).IsUnique();
        }
    }
}