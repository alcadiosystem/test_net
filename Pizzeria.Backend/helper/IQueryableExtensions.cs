
using Microsoft.EntityFrameworkCore;

namespace Pizzeria.Backend.helper;

public static class IQueryableExtensions
{
    public static async Task<PagedResult<T>> PaginarAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var totalRegistros = await query.CountAsync();
        var datos = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            TotalRegistros = totalRegistros,
            PaginaActual = pageNumber,
            TamanoPagina = pageSize,
            Datos = datos
        };
    }
}
