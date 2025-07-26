using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Backend.data;
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
        public async Task<IActionResult> GetAsync([FromQuery] string? name)
        {
            var query = _context.Productos.AsQueryable();

            if (name != null)
            {
                query = query.Where(x => x.Nombre.Contains(name));
            }
            var product = await query.ToListAsync();
            return Ok(product);
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