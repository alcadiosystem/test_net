
namespace Pizzeria.Backend.helper;

public class PagedResult<T>
{
    public int TotalRegistros { get; set; }
    public int PaginaActual { get; set; }
    public int TamanoPagina { get; set; }
    public List<T> Datos { get; set; } = new List<T>();
}
