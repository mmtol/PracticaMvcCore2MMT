using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2MMT.Filter;
using PracticaMvcCore2MMT.Models;
using PracticaMvcCore2MMT.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2MMT.Controllers
{
    public class UsuarioController : Controller
    {
        private RepositoryLibros repo;

        public UsuarioController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Perfil()
        {
            int id = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            Usuarios user = await repo.FindUsuarioAsync(id);
            return View(user);
        }
    }
}
