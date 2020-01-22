using System;
using System.IO;
using System.Linq;
using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Helper;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCentar.web.Controllers
{
    [Autorizacija(admin: true, clan: false, zaposlenik: true)]
    public class AdministracijaWebShopController : Controller
    {
        IWebShopService webShopService;
        IHostingEnvironment _environment;
        public AdministracijaWebShopController(IWebShopService _webShopService, IHostingEnvironment environment)
        {
            webShopService = _webShopService;
            _environment = environment;
        }
        public IActionResult PrikazKategorija()
        {
            AdministracijaPrikazKategorijaVM model = new AdministracijaPrikazKategorijaVM
            {
                Kategorije = webShopService.GetKategorije().Where(x => x.Obrisan == false).Select(k => new AdministracijaPrikazKategorijaVM.Kategorija
                {
                    ID = k.ID,
                    Naziv = k.Naziv,
                    Podkategorije = webShopService.GetPodkategorije().Where(x => x.KategorijaID == k.ID).Select(x => x.Naziv).ToList()
                }).ToList(),
            };
            return View("PrikazKategorija", model);
        }
        public IActionResult DodajKategoriju()
        {
            AdministracijaDodajKategorijuVM model = new AdministracijaDodajKategorijuVM();
            return View("DodajKategoriju", model);
        }
        public IActionResult SpremiKategoriju(AdministracijaDodajKategorijuVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("DodajKategoriju", model);
            }
            Kategorija kategorija = new Kategorija
            {
                Naziv = model.Naziv
            };
            webShopService.DodajKategoriju(kategorija);
            return RedirectToAction("KategorijaDetalji", new { id = kategorija.ID });
        }
        public IActionResult KategorijaDetalji(int id)
        {
            Kategorija kategorija = webShopService.GetKategorijaByID(id);
            if (kategorija == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaKategorijaDetaljiVM model = new AdministracijaKategorijaDetaljiVM
            {
                ID = kategorija.ID,
                Naziv = kategorija.Naziv
            };
            return View("KategorijaDetalji", model);
        }
        public IActionResult DodajStavku()
        {
            AdministracijaDodajStavkuVM model = new AdministracijaDodajStavkuVM
            {
                Kategorije = webShopService.GetKategorijeSelectListItem()
            };
            return View("DodajStavku", model);
        }
        public IActionResult SpremiStavku(AdministracijaDodajStavkuVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Kategorije = webShopService.GetKategorijeSelectListItem();
                return View("DodajStavku", model);
            }
            Stavka stavka = new Stavka
            {
                Naziv = model.Naziv,
                Cijena = model.Cijena,
                Opis = model.Opis,
                PodkategorijaID = model.PodkategorijaID
            };
            if (model.Slika != null)
            {
                stavka.Slika = model.Naziv.Replace(" ", "") + Path.GetExtension(model.Slika.FileName);
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images/Stavke/");
                string filePath = Path.Combine(uploadsFolder, stavka.Slika);
                model.Slika.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            webShopService.DodajStavku(stavka);
            TempData["novaStavka"] = stavka.Naziv;
            return RedirectToAction("PrikazStavke");
        }
        public IActionResult PrikazStavke(string naziv)
        {
            AdministracijaPrikazStavke model = new AdministracijaPrikazStavke();
            if (naziv == null)
            {
                model.Stavke = webShopService.GetStavke().Where(x => x.Obrisan == false).Select(x => new AdministracijaPrikazStavke.Row
                {
                    ID = x.ID,
                    Cijena = x.Cijena.ToString(),
                    Kategorija = x.Podkategorija.Kategorija.Naziv,
                    Naziv = x.Naziv,
                    Podkategorija = x.Podkategorija.Naziv,
                    Slika = x.Slika,
                    Skladiste = webShopService.GetSkladista(x.ID).Select(s => s.Velicina + "( " + s.Kolicina + " kom.)").ToList()
                }).ToList();
            }
            else
            {
                model.Stavke = webShopService.GetStavke().Where(x => x.Obrisan == false).Where(x => x.Naziv.ToLower().Contains(naziv.ToLower()) == true).Select(x => new AdministracijaPrikazStavke.Row
                {
                    ID = x.ID,
                    Cijena = x.Cijena.ToString(),
                    Kategorija = x.Podkategorija.Kategorija.Naziv,
                    Naziv = x.Naziv,
                    Podkategorija = x.Podkategorija.Naziv,
                    Slika = x.Slika,
                    Skladiste = webShopService.GetSkladista(x.ID).Select(s => s.Velicina + "( " + s.Kolicina + " kom.)").ToList()
                }).ToList();
            }
            if(TempData["novaStavka"] != null)
            {
                ViewBag.NovaStavka = TempData["novaStavka"].ToString();
            }
            else if(TempData["obrisanaStavka"] != null)
            {
                ViewBag.ObrisanaStavka = TempData["obrisanaStavka"].ToString();
            }
            return View("PrikazStavke", model);
        }
        public IActionResult UrediStavku(int id)
        {
            Stavka stavka = webShopService.StavkaFind(id);
            if (stavka == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaUrediStavkuVM model = new AdministracijaUrediStavkuVM
            {
                ID = stavka.ID,
                Cijena = stavka.Cijena,
                Naziv = stavka.Naziv,
                Kategorija = stavka.Podkategorija.Kategorija.Naziv,
                Podkategorija = stavka.Podkategorija.Naziv,
                Opis = stavka.Opis
            };
            return View("UrediStavku", model);
        }
        public IActionResult SpremiUrediStavku(AdministracijaUrediStavkuVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("UrediStavku", model);
            }
            Stavka stavka = webShopService.StavkaFind(model.ID);
            stavka.Cijena = model.Cijena;
            stavka.Opis = model.Opis;
            webShopService.UpdateStavka(stavka);
            return RedirectToAction("PrikazStavke");
        }
        public IActionResult ObrisiStavku(int id)
        {
            Stavka stavka = webShopService.StavkaFind(id);
            if (stavka == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            stavka.Obrisan = true;
            webShopService.UpdateStavka(stavka);
            TempData["obrisanaStavka"] = stavka.Naziv;
            return RedirectToAction("PrikazStavke");
        }
        public IActionResult PrikazNarudzbi()
        {
            AdministracijaPrikazNaruzbiVM model = new AdministracijaPrikazNaruzbiVM
            {
                Narudzbe = webShopService.GetNarudzbe().Where(x => x.RacunID == null).Select(x => new AdministracijaPrikazNaruzbiVM.Row
                {
                    ID = x.ID,
                    Cijena = x.Cijena.ToString(),
                    VrijemeNarudzbe = x.DatumVrijeme.ToString("dd.MM.yyyy - hh:mm"),
                    BrojStavki = webShopService.BrojStavki(x.ID).ToString()
                }).ToList()
            };
            return View("PrikazNarudzbi", model);
        }
        public IActionResult DetaljiNarudzba(int id)
        {
            Narudzba narudzba = webShopService.NarudzbaFind(id);
            if (narudzba == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaDetaljiNarudzbaVM model = new AdministracijaDetaljiNarudzbaVM
            {
                Adresa = narudzba.Adresa.Ulica + ", " + narudzba.Adresa.Grad + " " + narudzba.Adresa.PostanskiBroj,
                Kupac = narudzba.Korisnik.Ime + " " + narudzba.Korisnik.Prezime,
                Stavke = webShopService.GetNarudzbaStavke(narudzba.ID).Select(x => new AdministracijaDetaljiNarudzbaVM.Row
                {
                    Naziv = x.Stavka.Naziv + " - " + x.Velicina.Velicina,
                    Kategorija = x.Stavka.Podkategorija.Kategorija.Naziv + "( " + x.Stavka.Podkategorija.Naziv + ")",
                    Kolicina = x.Kolicina.ToString(),
                    UkupnaCijena = x.Cijena.ToString() + " KM",
                    Slika = x.Stavka.Slika
                }).ToList(),
                Isporuceno = narudzba.RacunID == null ? false : true,
                ID = narudzba.ID
            };
            return View("DetaljiNarudzba", model);
        }
        public IActionResult Isporuceno(int id)
        {
            Narudzba narudzba = webShopService.NarudzbaFind(id);
            if (narudzba == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            Racun racun = new Racun
            {
                ZaposlenikID = HttpContext.GetLogiraniKorisnik().Korisnik.ID,
                DatumVrijeme = DateTime.Now,
            };
            webShopService.DodajRacun(racun);
            narudzba.RacunID = racun.ID;
            webShopService.UpdateNarudzba(narudzba);
            return RedirectToAction("DetaljiNarudzba", new { id = narudzba.ID });
        }
        public IActionResult PrikazRacuna()
        {
            AdministracijaPrikazRacunaVM model = new AdministracijaPrikazRacunaVM
            {
                Racuni = webShopService.GetRacune().Select(x => new AdministracijaPrikazRacunaVM.Row
                {
                    ID = x.ID,
                    Cijena = webShopService.NarudzbaByRacun(x.ID).Cijena + " KM",
                    DatumIsporuke = x.DatumVrijeme.ToString("dd.MM.yyyy"),
                    VrijemeNarudzbe = webShopService.NarudzbaByRacun(x.ID).DatumVrijeme.ToString("dd.MM.yyyy - hh:mm"),
                    BrojStavki = webShopService.BrojStavki(webShopService.NarudzbaByRacun(x.ID).ID).ToString()
                }).ToList()
            };
            return View("PrikazRacuna", model);
        }
    }
}