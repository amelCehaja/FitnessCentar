using System.Linq;
using System.Text.RegularExpressions;
using FitnessCentar.data.EF;
using FitnessCentar.data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCentar.web.Controllers
{
    public class KlijentValidacijaController : Controller
    {
        private MyContext db;
        public KlijentValidacijaController(MyContext myContext)
        {
            db = myContext;
        }

        public IActionResult CheckEmail(string Email)
        {
            Match match = Regex.Match(Email, @"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,4}$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return Json($"Pogrešan email format! Format: mail@mail.com");
            }

            Korisnik k = db.Korisnik.SingleOrDefault(e => e.Email == Email);
            if (k != null)
            {
                return Json($"Email {Email} već postoji!");
            }

            return Json(true);
        }

        public IActionResult CheckUsername(string KorisnickoIme)
        {
            KorisnickiNalog kn = db.KorisnickiNalog.SingleOrDefault(x => x.KorisnickoIme == KorisnickoIme);
            if (kn == null)
            {
                return Json(true);
            }
            return Json($"Korisničko ime {KorisnickoIme} već postoji!");
        }
    }
}