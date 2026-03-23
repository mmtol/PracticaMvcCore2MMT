using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2MMT.Models;
using PracticaMvcCore2MMT.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2MMT.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;

        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string pass)
        {
            Usuarios usuario = await repo.LogInUsuarioAsync(email, pass);

            if (usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role
                );

                string nombre = usuario.Nombre;
                int idUsuario = usuario.IdUsuario;

                identity.AddClaim(new Claim(ClaimTypes.Name, nombre));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()));

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal
                );

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();

                var routeValues = new RouteValueDictionary();

                if (TempData["id"] != null)
                {
                    routeValues["id"] = idUsuario.ToString();
                }

                if (TempData["genero"] != null)
                {
                    routeValues["genero"] = TempData["genero"].ToString();
                }

                if (routeValues.Count > 0)
                {
                    return RedirectToAction(action, controller, routeValues);
                }
                else
                {
                    return RedirectToAction("Index", "Libros");
                }
            }
            else
            {
                ViewData["Mensaje"] = "Email o contraseña incorrectos, vuelva a intentarlo";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
