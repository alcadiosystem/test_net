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
        public DbSet<Ventas> Ventas { get; set; }

        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasIndex(usuario => usuario.DNI).IsUnique();
            modelBuilder.Entity<Producto>().HasIndex(producto => producto.Nombre).IsUnique();
            DisableCascadingDelete(modelBuilder);
        }
        //Permite quitar el eliminado en cascada
        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}