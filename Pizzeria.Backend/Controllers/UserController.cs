using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Backend.data;
using Pizzeria.Shared.Entites;

namespace Pizzeria.Backend.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UserController : ControllerBase
    {

        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string? search)
        {
            var query = _context.Usuarios.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(
                    us => us.Nombre.Contains(search) ||
                    us.DNI.ToString().Contains(search)
                );
            }

            var usuarios = await query.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUseriId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario(Usuario usuario)
        {
            try
            {
                var us = await _context.Usuarios.FirstOrDefaultAsync(u => u.DNI == usuario.DNI);
                if (us != null)
                {
                    var errorMessage = $"El usuario con DNI {usuario.DNI} ya existe, pruebe otro DNI";
                    return BadRequest(new { error = errorMessage });
                }
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return Ok(usuario);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutUsuario(Usuario usuario)
        {
            try
            {
                var us = await _context.Usuarios.FindAsync(usuario.Id);
                if (us == null)
                {
                    return NotFound();
                }

                us.DNI = usuario.DNI;
                us.Nombre = usuario.Nombre;

                await _context.SaveChangesAsync();
                return Ok(usuario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var us = await _context.Usuarios.FindAsync(id);
                if (us == null)
                {
                    return NotFound();
                }

                _context.Remove(us);
                await _context.SaveChangesAsync();
                return Ok("Eliminado");

            }
            catch (System.Exception e)
            {

                return BadRequest(e);
            }
        }

    }
}