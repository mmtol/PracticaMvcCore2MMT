using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PracticaMvcCore2MMT.Filter
{
    public class AuthorizeUsuariosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            string controller = context.RouteData.Values["controller"].ToString();
            string action = context.RouteData.Values["action"].ToString();

            var id = context.RouteData.Values["id"];
            var genero = context.RouteData.Values["genero"];

            ITempDataProvider provider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();
            var temp = provider.LoadTempData(context.HttpContext);
            temp["controller"] = controller;
            temp["action"] = action;

            if (id != null)
            {
                temp["id"] = id.ToString();
            }
            else
            {
                temp.Remove("id");
            }

            if (genero != null)
            {
                temp["genero"] = genero.ToString();
            }
            else
            {
                temp.Remove("genero");
            }

            provider.SaveTempData(context.HttpContext, temp);

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = GetRoute("Managed", "Login");
            }
        }

        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary
                (
                    new
                    {
                        controller = controller,
                        action = action
                    }
                );
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
