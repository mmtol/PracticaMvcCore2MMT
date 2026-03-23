using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2MMT.Data;
using PracticaMvcCore2MMT.Models;

namespace PracticaMvcCore2MMT.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;

        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        //sacar los generos
        //sacar los libros
        //sacar los detalles de un libro
        //login
        //perfil
        //hacer un pedido
        //sacar los pedidos de un usuario

        public async Task<int> GetMaxIdPedidoAsync() //unico
        {
            if (context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await context.Pedidos.MaxAsync(z => z.IdPedido) + 1;
            }
        }

        public async Task<int> GetMaxIdFacturaAsync() //repe
        {
            if (context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await context.Pedidos.MaxAsync(z => z.IdFactura) + 1;
            }
        }

        public async Task<List<Generos>> GetGenerosAsync()
        {
            var consulta = from datos in context.Generos
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<Libros>> GetLibrosAsync()
        {
            var consulta = from datos in context.Libros
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<Libros>> GetLibrosGeneroAsync(int idGenero)
        {
            var consulta = from datos in context.Libros
                           where datos.IdGenero == idGenero
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Libros> FindLibroAsync(int idLibro)
        {
            var consulta = from datos in context.Libros
                           where datos.IdLibro == idLibro
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<Usuarios> LogInUsuarioAsync(string email, string pass)
        {
            Usuarios user = await context.Usuarios.FirstOrDefaultAsync(e => e.Email == email && e.Pass == pass);
            return user;
        }

        public async Task<Usuarios> FindUsuarioAsync(int idUsuario)
        {
            var consulta = from datos in context.Usuarios
                           where datos.IdUsuario == idUsuario
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task CreatePedidoAsync(int idFactura, DateTime fecha, int idLibro, int idUsuario, int cantidad)
        {
            int idPedido = await GetMaxIdPedidoAsync();

            Pedido pedido = new Pedido
            {
                IdPedido = idPedido,
                IdFactura = idFactura,
                Fecha = fecha,
                IdLibro = idLibro,
                IdUsuario = idUsuario,
                Cantidad = cantidad
            };
            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();
        }

        public async Task<List<VistaPedidos>> FindVistaPedidosUsuarioAsync(int idUsuario)
        {
            var consulta = from datos in context.VistaPedidos
                           where datos.IdUsuario == idUsuario
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<Libros>> GetLibrosSessionAsync(List<Libros> libros)
        {
            var consulta = from datos in this.context.Libros
                           where libros.Select(c => c.IdLibro).Contains(datos.IdLibro)
                           select datos;
            return await consulta.ToListAsync();
        }
    }
}
