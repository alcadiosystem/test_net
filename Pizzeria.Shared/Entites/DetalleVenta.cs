
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizzeria.Shared.Entites;

public class DetalleVenta
{
    public int Id { get; set; }

    [ForeignKey("Producto")]
    public int IdProducto { get; set; }
    public Producto Producto { get; set; } = null!;

    [ForeignKey("Ventas")]
    public int IdVentas { get; set; }
    public Ventas Ventas { get; set; } = null!;

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioUnitario { get; set; }

    public decimal Total => Cantidad * PrecioUnitario;

}
