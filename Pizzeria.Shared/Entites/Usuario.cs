using System.ComponentModel.DataAnnotations;


namespace Pizzeria.Shared.Entites;

public class Usuario
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Nombre { get; set; } = null!;

    [Required]
    public int DNI { get; set; }

    public ICollection<Ventas>? Ventas { get; set; }

    //Propiedad de lectura para las ventas
    public int VentasCount => Ventas == null ? 0 : Ventas.Count;
}
