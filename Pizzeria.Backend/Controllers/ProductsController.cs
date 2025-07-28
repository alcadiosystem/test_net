using Microsoft.AspNetCore.Mvc;
using Pizzeria.Backend.data;
using Pizzeria.Backend.helper;
using Pizzeria.Shared.Entites;

namespace Pizzeria.Backend.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.Productos.AsQueryable();

            // Filtro por nombre
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Nombre.Contains(name));
            }

            // Ordenar por nombre
            query = query.OrderBy(x => x.Nombre);

            // Usar el helper de paginación
            var resultado = await query.PaginarAsync(pageNumber, pageSize);

            return Ok(resultado);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Producto product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(Producto product)
        {
            try
            {
                var currentProduct = await _context.Productos.FindAsync(product.Id);
                if (currentProduct == null)
                {
                    return NotFound();
                }
                currentProduct.Nombre = product.Nombre;
                currentProduct.Descripcion = product.Descripcion;
                currentProduct.precio = product.precio;

                await _context.SaveChangesAsync();
                return Ok(currentProduct);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                _context.Remove(producto);
                await _context.SaveChangesAsync();
                return Ok("Eliminado");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}