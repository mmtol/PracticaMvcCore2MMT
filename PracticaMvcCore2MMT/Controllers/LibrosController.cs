using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2MMT.Extensions;
using PracticaMvcCore2MMT.Filter;
using PracticaMvcCore2MMT.Models;
using PracticaMvcCore2MMT.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2MMT.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;

        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index(int? genero, int? idlibro)
        {
            if (idlibro != null)
            {
                List<Libros> carrito;
                if (HttpContext.Session.GetObject<List<Libros>>("libros") != null)
                {
                    carrito = HttpContext.Session.GetObject<List<Libros>>("libros");
                }
                else
                {
                    carrito = new List<Libros>();
                }

                Libros libro = await repo.FindLibroAsync(idlibro.Value);
                carrito.Add(libro);
                HttpContext.Session.SetObject("libros", carrito);
                return RedirectToAction("Carrito");
            }

            List<Libros> libros = new List<Libros>();

            if (genero != null)
            {
                libros = await repo.GetLibrosGeneroAsync((int)genero);
            }
            else
            {
                libros = await repo.GetLibrosAsync();
            }

            return View(libros);
        }

        public async Task<IActionResult> Details(int idlibro)
        {
            Libros libro = await repo.FindLibroAsync(idlibro);
            return View(libro);
        }

        public async Task<IActionResult> Carrito(int? ideliminar)
        {
            List<Libros> libros = HttpContext.Session.GetObject<List<Libros>>("libros");
            if (libros == null)
            {
                return View();
            }
            else
            {
                if (ideliminar != null)
                {
                    Libros libro = libros.Find(z => z.IdLibro == ideliminar.Value);
                    libros.Remove(libro);
                    if (libros.Count == 0)
                    {
                        HttpContext.Session.Remove("libros");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("libros", libros);
                    }
                }
                List<Libros> total = await this.repo.GetLibrosSessionAsync(libros);
                return View(total);
            }
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> CreateCompra()
        {
            List<Libros> libros = HttpContext.Session.GetObject<List<Libros>>("libros");
            int idFactura = await repo.GetMaxIdFacturaAsync();
            int idUsuario = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            int cantidad = 1;
            DateTime fecha = DateTime.Now;

            foreach (Libros libro in libros)
            {
                await repo.CreatePedidoAsync(idFactura, fecha, libro.IdLibro, idUsuario, cantidad);
            }

            HttpContext.Session.Remove("libros");
            return RedirectToAction("VistaPedidosUsuario");
        }

        public async Task<IActionResult> VistaPedidosUsuario()
        {
            int idUsuario = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<VistaPedidos> vistaPedidos = await repo.FindVistaPedidosUsuarioAsync(idUsuario);
            return View(vistaPedidos);
        }
    }
}
