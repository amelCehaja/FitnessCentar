using System;
using System.IO;
using System.Linq;
using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Helper;
using FitnessCentar.web.Helpers;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FitnessCentar.web.Controllers
{
    [Autorizacija(admin: true, clan: false, zaposlenik: true)]
    public class AdministracijaClanController : Controller
    {
        IClanService clanService;
        IHelper helper;
        private readonly IHostingEnvironment _environment;
        public AdministracijaClanController(IClanService _clanService, IHelper _helper, IHostingEnvironment environment)
        {
            clanService = _clanService;
            helper = _helper;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DodajClana()
        {
            AdministracijaDodajClanaVM model = new AdministracijaDodajClanaVM
            {
                spol = helper.GenereateSpolList(),
                DatumRodenja = DateTime.Today,
                clanarine = clanService.GetTipoviClanarineSelectList(),
                DatumDodavanja = DateTime.Now.Date
            };

            return View("DodajClanaForma", model);
        }
        public IActionResult SpremiClan(AdministracijaDodajClanaVM data)
        {
            if (!ModelState.IsValid)
            {
                data.spol = helper.GenereateSpolList();
                data.clanarine = clanService.GetTipoviClanarineSelectList();
                return View("DodajClanaForma", data);
            }

            Random rnd = new Random();
            string tempUsername = data.Ime.ToLower() + "." + data.Prezime.ToLower();
            KorisnickiNalog kn = new KorisnickiNalog
            {
                KorisnickoIme = clanService.IsUsernameUnique(tempUsername) == true ? tempUsername : tempUsername + rnd.Next(1, 99).ToString(),
                Tip = "clan",
                Lozinka = Guid.NewGuid().ToString()
            };
            clanService.DodajKorisnickiNalog(kn);
            Korisnik clan = new Korisnik
            {
                Ime = data.Ime,
                Prezime = data.Prezime,
                BrojKartice = data.BrojKartice,
                BrojTelefona = data.BrojTelefona,
                DatumRodenja = data.DatumRodenja,
                Email = data.Email,
                Slika = "default.jpg",
                Spol = data.Spol,
                KorisnickiNalogID = kn.ID
            };
            clanService.DodajClana(clan);
            HttpContext.Session.SetInt32("photoAddID", clan.ID);
            Clanarina clanarina = new Clanarina
            {
                ClanID = clan.ID,
                DatumDodavanja = data.DatumDodavanja,
                DatumIsteka = data.DatumIsteka,
                TipClanarineID = data.TipClanarineID
            };
            clanService.DodajClanarinu(clanarina);
            return RedirectToAction("ForgotPassword", "Autentifikacija", new { EmailID = clan.Email, newUserType = "clan" });
        }
        public IActionResult SpremiEditClan(AdministracijaEditClanaVM model)
        {
            if (!ModelState.IsValid)
            {
                model.spol = helper.GenereateSpolList();
                return View("EditClan", model);
            }
            Korisnik clan = clanService.ClanFind(model.ID);

            clan.ID = model.ID;
            clan.BrojTelefona = model.BrojTelefona;
            clan.DatumRodenja = model.DatumRodenja;
            clan.Ime = model.Ime;
            clan.Prezime = model.Prezime;
            clan.Spol = model.Spol;
            clan.JMBG = model.JMBG;
            clanService.ClanUpdate(clan);
            return RedirectToAction("ClanDetalji", new { id = clan.ID });
        }
        public IActionResult EditClan(int id)
        {
            Korisnik clan = clanService.ClanFind(id);
            if (clan == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaEditClanaVM model = new AdministracijaEditClanaVM
            {
                ID = clan.ID,
                BrojKartice = clan.BrojKartice,
                BrojTelefona = clan.BrojTelefona,
                DatumRodenja = clan.DatumRodenja,
                Ime = clan.Ime,
                Prezime = clan.Prezime,
                Spol = clan.Spol,
                spol = helper.GenereateSpolList()
            };
            return View("EditClan", model);
        }
        public IActionResult PrikazClanova()
        {
            AdministracijaPrikazClanovaVM data = new AdministracijaPrikazClanovaVM
            {
                BrojAktivnihClanova = clanService.BrojAktivnihClanarina(),
                UkupniBrojClanova = clanService.UkupanBrojClanova(),
                ClanarineAN = helper.ClanarineAN()
            };
            return View("PrikazClanova", data);
        }
        public IActionResult PrikazPrisutnihClanova()
        {
            return View("PrikazPrisutnihClanova");
        }
        public IActionResult ObrisiKorisnika(int id)
        {
            Korisnik korisnik = clanService.ClanFind(id);
            if (korisnik == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            korisnik.Obrisan = true;
            clanService.ClanUpdate(korisnik);
            return RedirectToAction("PrikazClanova");
        }
        public IActionResult DodajClanarinu(int id)
        {
            Korisnik clan = clanService.ClanFind(id);
            if (clan == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaDodajClanarinuVM model = new AdministracijaDodajClanarinuVM
            {
                ClanID = id,
                ImePrezime = clan.Ime + " " + clan.Prezime,
                TipoviClanarine = clanService.GetTipoviClanarineSelectList(),
                DatumDodavanja = DateTime.Now.Date
            };
            return View("DodajClanarinu", model);
        }
        public IActionResult SpremiClanarinu(AdministracijaDodajClanarinuVM model)
        {
            if (ModelState.IsValid)
            {
                Clanarina clanarina = new Clanarina
                {
                    ClanID = model.ClanID,
                    TipClanarineID = model.TipClanarineID,
                    DatumDodavanja = model.DatumDodavanja,
                    DatumIsteka = model.DatumIsteka,
                };
                clanService.DodajClanarinu(clanarina);
                return RedirectToAction("ClanDetalji", new { id = clanarina.ClanID });
            }
            model.TipoviClanarine = clanService.GetTipoviClanarineSelectList();
            return View("DodajClanarinu", model);
        }
        public IActionResult PrikazClanarina()
        {
            AdministracijaPrikazClanarinaVMs model = new AdministracijaPrikazClanarinaVMs
            {
                tipoviClanarine = clanService.GetTipoviClanarine(),
                statusi = helper.ClanarineAN(),
                UkupnoAktivnih = clanService.BrojAktivnihClanarina()
            };
            model.tipoviClanarine.Insert(0, new SelectListItem
            {
                Text = "Svi tipovi",
                Value = "sve"
            });
            return View(model);
        }
        public IActionResult PrikazTipovaClanarina()
        {
            AdministracijaPrikazTipovaClanarinaVM tipoviClanarine = new AdministracijaPrikazTipovaClanarinaVM
            {
                tipoviClanarine = clanService.GetAllTipoviClanarine().Where(x => x.Obrisan == false).Select(t => new AdministracijaPrikazTipovaClanarinaVM.TipClanarine
                {
                    ID = t.ID,
                    Cijena = t.Cijena.ToString() + " KM",
                    Naziv = t.Naziv,
                    VrijemeTrajanja = t.VrijemeTrajanja + " Dana"
                }).ToList()
            };
            if (TempData["obrisanTip"] != null)
            {
                ViewBag.ObrisanTip = TempData["obrisanTip"].ToString();
            }
            else if (TempData["noviTipClanarine"] != null)
            {
                ViewBag.noviTipClanarine = TempData["noviTipClanarine"].ToString();
            }
            return View("PrikazTipovaClanarina", tipoviClanarine);

        }
        public IActionResult DodajTipClanarine()
        {
            AdministracijaDodajTipClanarineVM model = new AdministracijaDodajTipClanarineVM();
            return View("DodajTipClanarine", model);
        }
        public IActionResult EditTipClanarine(int id)
        {
            TipClanarine tipClanarine = clanService.TipClanarineFind(id);
            if (tipClanarine == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaEditTipClanarineVM model = new AdministracijaEditTipClanarineVM
            {
                ID = tipClanarine.ID,
                Cijena = tipClanarine.Cijena,
                Naziv = tipClanarine.Naziv,
                Trajanje = tipClanarine.VrijemeTrajanja
            };
            return View("EditTipClanarine", model);
        }
        public IActionResult SnimiTipClanarine(AdministracijaDodajTipClanarineVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("DodajTipClanarine", model);
            }
            TipClanarine tipClanarine = new TipClanarine
            {
                Naziv = model.Naziv,
                Cijena = model.Cijena,
                VrijemeTrajanja = model.Trajanje
            };
            TempData["noviTipClanarine"] = tipClanarine.Naziv;
            clanService.DodajTipClanarine(tipClanarine);
            return Redirect("PrikazTipovaClanarina");
        }
        public IActionResult SnimiEditTipClanarine(AdministracijaEditTipClanarineVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("DodajTipClanarine", model);
            }
            TipClanarine tipClanarine = clanService.TipClanarineFind(model.ID);
            tipClanarine.Cijena = model.Cijena;
            tipClanarine.VrijemeTrajanja = model.Trajanje;
            clanService.UpdateTipClanarine(tipClanarine);
            return RedirectToAction("PrikazTipovaClanarina");
        }
        public IActionResult ObrisiTipClanarine(int id)
        {
            TipClanarine tipClanarine = clanService.TipClanarineFind(id);
            if (tipClanarine == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            tipClanarine.Obrisan = true;
            clanService.UpdateTipClanarine(tipClanarine);
            TempData["obrisanTip"] = tipClanarine.Naziv;
            return RedirectToAction("PrikazTipovaClanarina");
        }
        public IActionResult ClanDetalji(int id)
        {
            AdministracijaDetaljiClanaVM model = new AdministracijaDetaljiClanaVM
            {
                clan = clanService.ClanFind(id),
            };
            if (model.clan == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            model.Clanarine = clanService.GetClanarineByClanID(id).OrderByDescending(x => x.DatumDodavanja).Select(x => new AdministracijaDetaljiClanaVM.Row
            {
                DatumDodavanja = x.DatumDodavanja.ToString("dd.MM.yyyy"),
                DatumIsteka = x.DatumIsteka.ToString("dd.MM.yyyy"),
                TipClanarine = x.TipClanarine.Naziv
            }).ToList();
            model.clanarina = clanService.AktivnaClanarina(id);
            model.DatumPrveClanarine = clanService.DatumPrveClanarine(id);
            return View("ClanDetalji", model);
        }
        public IActionResult AddClanPhoto()
        {
            return View();
        }
        public IActionResult Capture(string name)
        {
            //preuzimanje ID clana iz sesije
            int? ID = HttpContext.Session.GetInt32("photoAddID");
            var files = HttpContext.Request.Form.Files;
            if (files != null && ID != null)
            {
                int _id = ID ?? 0;
                Korisnik clan = clanService.ClanFind(_id);
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //dodavanje naziva slike
                        clan.Slika = clan.KorisnickiNalog.KorisnickoIme + Path.GetExtension(file.FileName);
                        //putanja slike
                        var filepath = Path.Combine(_environment.WebRootPath, "images/Clanovi/") + $@"\{clan.Slika}";
                        //spremanje u folder
                        StoreInFolder(file, filepath);
                    }
                }
                clanService.ClanUpdate(clan);
            }
            //brisanje ID clana iz sesije
            HttpContext.Session.Remove("photoAddID");
            return RedirectToAction("ClanDetalji", new { id = ID });
        }
        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
        public IActionResult UpdateBrojKartice(int id)
        {
            Korisnik korisnik = clanService.ClanFind(id);
            if (korisnik == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            UpdateBrojKarticeVM model = new UpdateBrojKarticeVM
            {
                ID = korisnik.ID,
                ImePrezime = korisnik.Ime + " " + korisnik.Prezime,
                BrojKartice = korisnik.BrojKartice
            };
            return View("UpdateBrojKartice", model);
        }
        public IActionResult SpremiBrojKartice(UpdateBrojKarticeVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateBrojKartice", model);
            }
            Korisnik korisnik = clanService.ClanFind(model.ID);
            korisnik.BrojKartice = model.BrojKartice;
            clanService.ClanUpdate(korisnik);
            return RedirectToAction("ClanDetalji", new { id = model.ID });
        }
    }
}