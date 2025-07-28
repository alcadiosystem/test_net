
namespace Pizzeria.Shared.models;

public class ActualizarDetalleVentaRequest
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public decimal Total { get; set; }
}
