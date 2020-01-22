using FitnessCentar.data.EF;
using FitnessCentar.data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessCentar.web.Helper
{
    public class AutorizacijaAttribute : TypeFilterAttribute
    {
        public AutorizacijaAttribute(bool admin,bool clan,bool zaposlenik) : base(typeof(AuthorizeImpl))
        {
            Arguments = new object[] {admin,clan,zaposlenik };
        }
    }

    public class AuthorizeImpl:IAsyncActionFilter
    {
        public AuthorizeImpl(bool admin,bool clan,bool zaposlenik)
        {
            _clan = clan;
            _zaposlenik = zaposlenik;
            _admin = admin;
        }
        private readonly bool _clan;
        private readonly bool _zaposlenik;
        private readonly bool _admin;
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            KorisnickiNalog korisnickiNalog = context.HttpContext.GetLogiraniKorisnik();
            if (korisnickiNalog == null)
            {
                if (context.Controller is Controller controller)
                {
                    controller.TempData["error_poruka"] = "Niste logirani!";
                }
                context.Result = new RedirectToActionResult("Index","Home",new {area ="" });
                return;
            }

            //MyContext db = context.HttpContext.RequestServices.GetService<MyContext>(); // if needed

            if (_clan && korisnickiNalog.Tip == "clan")
            {
                await next();
                return;
            }

            if (_zaposlenik && korisnickiNalog.Tip == "zaposlenik")
            {
                await next();
                return;
            }

            if(_admin && korisnickiNalog.Tip == "admin")
            {
                await next();
                return;
            }
            if (context.Controller is Controller c1)
            {
                c1.TempData["error_poruka"] = "Nemate pravo pristupa!";
            }
            context.Result = new RedirectToActionResult("Index","Home",new {area="" });
        }
    }
}
