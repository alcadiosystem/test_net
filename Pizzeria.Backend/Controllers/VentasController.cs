using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Backend.data;
using Pizzeria.Shared.Entites;
using Pizzeria.Shared.models;

namespace Pizzeria.Backend.Controllers;

[ApiController]
[Route("api/ventas")]
public class VentasController : ControllerBase
{

    private readonly DataContext _context;

    public VentasController(DataContext context)
    {
        _context = context;
    }

    //Devuelve una lista de ventas.
    [HttpGet]
    public async Task<IActionResult> GetVentas(
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin,
        [FromQuery] string? search
    )
    {
        var query = _context.Ventas
            .Include(v => v.Usuario) // Incluye datos del usuario
            .AsQueryable();

        // Filtro por rango de fechas
        if (fechaInicio.HasValue)
            query = query.Where(v => v.Fecha >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(v => v.Fecha <= fechaFin.Value);

        // Filtro por nombre o DNI
        if (!string.IsNullOrWhiteSpace(search))
        {
            if (int.TryParse(search, out int dni))
            {
                // Si el search es numérico, filtra también por DNI exacto
                query = query.Where(v => v.Usuario.Nombre.Contains(search) || v.Usuario.DNI == dni);
            }
            else
            {
                query = query.Where(v => v.Usuario.Nombre.Contains(search));
            }
        }
        var ventas = await query
        .Select(v => new
        {
            Id = v.Id,
            UsuarioId = v.IdUsuario,
            NombreUsuario = v.Usuario.Nombre,
            Fecha = v.Fecha,
            Total = v.Total
        })
        .ToListAsync();

        return Ok(ventas);
    }

    //Devuelve los detalles de una venta específica por su ID.
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVentaById(int id)
    {
        var venta = await _context.Ventas
            .Include(v => v.Usuario) // Incluye los datos del usuario
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venta == null)
        {
            return NotFound(new { message = $"No se encontró la venta con ID {id}." });
        }

        var result = new
        {
            Id = venta.Id,
            UsuarioId = venta.IdUsuario,
            NombreUsuario = venta.Usuario.Nombre,
            Fecha = venta.Fecha,
            Total = venta.Total
        };

        return Ok(result);
    }


    //Recibe los detalles de la venta a crear en el cuerpo del request.
    [HttpPost]
    public async Task<IActionResult> PostVenta([FromBody] CrearVentaRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Buscar o crear usuario
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.DNI == request.DNI);

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Nombre = request.NombreUsuario,
                    DNI = request.DNI
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }

            // 2. Crear la venta
            var venta = new Ventas
            {
                IdUsuario = usuario.Id,
                Fecha = DateTime.UtcNow,
                Total = 0 // se actualizará luego
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            // 3. Crear detalles
            var detalles = new List<DetalleVenta>();
            foreach (var item in request.Detalles)
            {
                // Obtener precio del producto
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null)
                {
                    return BadRequest(new { message = $"Producto con ID {item.ProductoId} no existe." });
                }

                var detalle = new DetalleVenta
                {
                    IdVentas = venta.Id,
                    IdProducto = producto.Id,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.precio
                };

                detalles.Add(detalle);
            }

            _context.DetalleVentas.AddRange(detalles);
            await _context.SaveChangesAsync();

            // Calcular total de la venta
            venta.Total = detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            _context.Ventas.Update(venta);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            // Respuesta
            return Ok(new
            {
                Id = venta.Id,
                UsuarioId = usuario.Id,
                VentaDetalleId = detalles.First().Id,
                NombreUsuario = usuario.Nombre,
                Fecha = venta.Fecha,
                Total = venta.Total
            });
        }
        catch (System.Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutVenta(int id, [FromBody] ActualizarVentaRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new { message = "El ID de la venta en la URL no coincide con el del cuerpo." });
        }

        try
        {
            var venta = await _context.Ventas
            .Include(v => v.Usuario)
            .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
            {
                return NotFound(new { message = $"No se encontró la venta con ID {id}." });
            }

            // Validar si el UsuarioId ha cambiado
            if (venta.IdUsuario != request.UsuarioId)
            {
                var nuevoUsuario = await _context.Usuarios.FindAsync(request.UsuarioId);
                if (nuevoUsuario == null)
                {
                    return BadRequest(new { message = $"El usuario con ID {request.UsuarioId} no existe." });
                }
                venta.IdUsuario = nuevoUsuario.Id;
                venta.Usuario = nuevoUsuario;
            }

            // Actualizar nombre de usuario si existe y es diferente
            if (venta.Usuario != null && venta.Usuario.Nombre != request.NombreUsuario)
            {
                venta.Usuario.Nombre = request.NombreUsuario;
            }

            // Actualizar la fecha y el total
            venta.Fecha = request.Fecha;
            venta.Total = request.Total;

            await _context.SaveChangesAsync();

            // Respuesta con los campos requeridos
            return Ok(new
            {
                Id = venta.Id,
                UsuarioId = venta.IdUsuario,
                NombreUsuario = venta.Usuario?.Nombre,
                Fecha = venta.Fecha,
                Total = venta.Total
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarVenta(int id)
    {
        var venta = await _context.Ventas
            .Include(v => v.DetalleVentas)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venta == null)
        {
            return NotFound(new { message = $"No se encontró la venta con ID {id}." });
        }

        // Eliminar los detalles primero (por DeleteBehavior.Restrict)
        if (venta.DetalleVentas != null && venta.DetalleVentas.Any())
        {
            _context.DetalleVentas.RemoveRange(venta.DetalleVentas);
        }

        // Eliminar la venta
        _context.Ventas.Remove(venta);

        await _context.SaveChangesAsync();

        return Ok(new { message = $"La venta con ID {id} fue eliminada correctamente." });
    }


}

