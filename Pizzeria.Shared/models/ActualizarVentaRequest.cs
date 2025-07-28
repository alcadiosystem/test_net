
namespace Pizzeria.Shared.models;

public class ActualizarVentaRequest
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
}
