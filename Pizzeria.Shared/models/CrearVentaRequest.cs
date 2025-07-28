namespace Pizzeria.Shared.models;

public class CrearVentaRequest
{
    public string NombreUsuario { get; set; } = null!;
    public int DNI { get; set; }
    public List<DetalleVentaItem> Detalles { get; set; } = new();
}
