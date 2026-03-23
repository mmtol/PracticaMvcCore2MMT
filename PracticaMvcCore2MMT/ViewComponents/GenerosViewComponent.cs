using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2MMT.Models;
using PracticaMvcCore2MMT.Repositories;

namespace PracticaMvcCore2MMT.ViewComponents
{
    public class GenerosViewComponent : ViewComponent
    {
        private RepositoryLibros repo;

        public GenerosViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Generos> generos = await repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
