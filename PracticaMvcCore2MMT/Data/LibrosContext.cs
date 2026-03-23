using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2MMT.Models;

namespace PracticaMvcCore2MMT.Data
{
    public class LibrosContext : DbContext
    {
        public LibrosContext(DbContextOptions<LibrosContext> options) : base(options) { }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<VistaPedidos> VistaPedidos { get; set; }
    }
}
