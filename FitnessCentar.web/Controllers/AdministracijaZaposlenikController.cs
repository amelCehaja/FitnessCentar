using System;
using System.Linq;
using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Helper;
using FitnessCentar.web.Helpers;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCentar.web.Controllers
{
    [Autorizacija(admin: true, clan: false, zaposlenik: false)]
    public class AdministracijaZaposlenikController : Controller
    {
        IZaposlenikService service;
        IHelper helper;
        IClanService clanService;
        public AdministracijaZaposlenikController(IZaposlenikService _service, IHelper _helper, IClanService _clanService)
        {
            service = _service;
            helper = _helper;
            clanService = _clanService;
        }
        public IActionResult DodajZaposlenika()
        {
            AdministracijaDodajZaposlenikaVM model = new AdministracijaDodajZaposlenikaVM
            {
                spol = helper.GenereateSpolList(),
                DatumRodenja = DateTime.Today
            };
            return View("DodajZaposlenika", model);
        }
        public IActionResult SpremiZaposlenika(AdministracijaDodajZaposlenikaVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("DodajZaposlenika", model);
            }

            string tempUserName = model.Ime.ToLower() + "." + model.Prezime.ToLower();
            Random rand = new Random();
            KorisnickiNalog korisnickiNalog = new KorisnickiNalog
            {
                KorisnickoIme = clanService.IsUsernameUnique(tempUserName) == true ? tempUserName : tempUserName + rand.Next(1, 99).ToString(),
                Tip = "zaposlenik",
                Lozinka = Guid.NewGuid().ToString()
            };
            clanService.DodajKorisnickiNalog(korisnickiNalog);
            Korisnik zaposlenik = new Korisnik
            {
                Ime = model.Ime,
                Prezime = model.Prezime,
                BrojTelefona = model.BrojTelefona,
                DatumRodenja = model.DatumRodenja,
                JMBG = model.JMBG,
                Spol = model.Spol,
                Email = model.Email,
                KorisnickiNalogID = korisnickiNalog.ID
            };
            service.DodajZaposlenika(zaposlenik);
            return RedirectToAction("ForgotPassword", "Autentifikacija", new { EmailID = zaposlenik.Email, newUserType = "zaposlenik" });
        }
        public IActionResult ObrisiZaposlenika(int id)
        {
            Korisnik zaposlenik = service.ZaposlenikFind(id);
            if (zaposlenik == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            service.ObrisiZaposlenik(zaposlenik);
            return RedirectToAction("PrikazZaposlenika");
        }
        public IActionResult PrikazZaposlenika()
        {
            AdministracijPrikazZaposlenikaVM model = new AdministracijPrikazZaposlenikaVM();
            model.zaposlenici = service.GetKorisnike().Where(x => x.Obrisan == false).Select(z => new AdministracijPrikazZaposlenikaVM.Zaposlenik
            {
                ImePrezime = z.Ime + " " + z.Prezime,
                DatumRodenja = z.DatumRodenja.ToString("dd.MM.yyyy"),
                ID = z.ID
            }).ToList();
            return View("PrikazZaposlenika", model);
        }
    }
}