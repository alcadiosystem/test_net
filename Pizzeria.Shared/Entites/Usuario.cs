using System.ComponentModel.DataAnnotations;


namespace Pizzeria.Shared.Entites
{
    public class Usuario
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Nombre { get; set; } = null!;

        
        [Required]
        public int DNI { get; set; }
    }
}