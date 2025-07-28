
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizzeria.Shared.Entites;

public class Ventas
{
    public int Id { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }
    public Usuario Usuario { get; set; } = null!;


    [Required]
    public DateTime Fecha { get; set; } = DateTime.UtcNow; //Establece la fecha de ventas actual del servidor

    [Column(TypeName = "decimal(18,2)")]
    [Required]
    public decimal Total { get; set; }

    public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();

    //Campo Calculado total venta
    public decimal TotalG => DetalleVentas == null ? 0 : DetalleVentas.Sum(x => x.Cantidad * x.PrecioUnitario);
}
