using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Pizzeria.Shared.Entites;

public class Producto
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    [Required]
    public Decimal precio { get; set; }

    [JsonIgnore]
    public ICollection<DetalleVenta>? DetalleVentas { get; set; }
}
