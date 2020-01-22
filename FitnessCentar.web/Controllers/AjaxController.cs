using System;
using System.Collections.Generic;
using System.Linq;
using FitnessCentar.data.Models;
using FitnessCentar.web.ViewModels.AjaxVMs;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessCentar.service.Interfaces;
using Microsoft.AspNetCore.Http;
using FitnessCentar.web.Helper;

namespace FitnessCentar.web.Controllers
{
    [Autorizacija(admin: true, clan: false, zaposlenik: true)]
    public class AjaxController : Controller
    {
        IAjaxService ajaxService;
        IClanService clanService;
        IPlanIProgramService planIProgramService;
        IWebShopService webShopService;
        public AjaxController(IAjaxService _ajaxService, IClanService _clanService, IPlanIProgramService _planIProgramService, IWebShopService _webShopService)
        {
            ajaxService = _ajaxService;
            clanService = _clanService;
            planIProgramService = _planIProgramService;
            webShopService = _webShopService;
        }
        public int TrajanjeClanarine(int id)
        {
            int tranjanje = ajaxService.TrajanjeTipaClanarine(id);
            return tranjanje;
        }
        public IActionResult PrikazClanarina(string tipClanarine, string status)
        {
            List<Clanarina> temp = new List<Clanarina>();
            AjaxPrikazClanarinaVM clanarine = new AjaxPrikazClanarinaVM
            {
                clanovi = new List<AjaxPrikazClanarinaVM.Clanarina>()
            };
            temp = ajaxService.GetClanarineByStatus(status).OrderByDescending(X => X.DatumDodavanja).ToList();
            foreach (var x in temp)
            {
                if (x.TipClanarine.ID.ToString() == tipClanarine || tipClanarine == "sve")
                {
                    clanarine.clanovi.Add(new AjaxPrikazClanarinaVM.Clanarina
                    {
                        ImePrezime = x.Clan.Ime + " " + x.Clan.Prezime,
                        TipClanarine = x.TipClanarine.Naziv,
                        DatumDodavanja = x.DatumDodavanja.Date.ToString("dd.MM.yyyy"),
                        DatumIsteka = x.DatumIsteka.Date.ToString("dd.MM.yyyy")
                    });
                }
            }
            return PartialView("PrikazClanarina", clanarine);
        }
        public IActionResult PrikazClanova(string imePrezime, string aktivnaDN)
        {
            AjaxPrikazClanova _clanovi = new AjaxPrikazClanova
            {
                clanovi = new List<AjaxPrikazClanova.Clan>()
            };
            AjaxPrikazClanova model = new AjaxPrikazClanova();
            model.clanovi = ajaxService.PrikazClanovaPoImenu(imePrezime).Select(x => new AjaxPrikazClanova.Clan
            {
                id = x.ID,
                Email = x.Email,
                ImePrezime = x.Ime + " " + x.Prezime,
                Slika = x.Slika
            }).ToList();

            if (model.clanovi != null)
            {
                foreach (var clan in model.clanovi)
                {
                    var _aktivnaClanarina = clanService.AktivnaClanarina(clan.id);
                    if (aktivnaDN == "sve")
                    {
                        if (_aktivnaClanarina == null)
                        {
                            clan.datumIstekaClanarine = "---";
                            clan.TipClanarine = "---";
                            _clanovi.clanovi.Add(clan);
                        }
                        else
                        {
                            clan.datumIstekaClanarine = _aktivnaClanarina.DatumIsteka.ToString("dd/MM/yyyy");
                            clan.TipClanarine = _aktivnaClanarina.TipClanarine.Naziv;
                            _clanovi.clanovi.Add(clan);
                        }
                    }
                    else if (aktivnaDN == "da" && _aktivnaClanarina != null)
                    {
                        clan.datumIstekaClanarine = _aktivnaClanarina.DatumIsteka.ToString("dd/MM/yyyy");
                        clan.TipClanarine = _aktivnaClanarina.TipClanarine.Naziv;
                        _clanovi.clanovi.Add(clan);
                    }
                    else if (aktivnaDN == "ne" && _aktivnaClanarina == null)
                    {
                        clan.datumIstekaClanarine = "---";
                        clan.TipClanarine = "---";
                        _clanovi.clanovi.Add(clan);
                    }
                }
            }
            return PartialView("PrikazClanova", _clanovi);
        }
        public string PodkategorijeList(int KategorijaID)
        {
            List<Podkategorija> podkategorije = ajaxService.GetPodkategorijeByKategorijaID(KategorijaID).Where(x => x.Obrisan == false).ToList();
            string html = "";
            foreach (var podkategorija in podkategorije)
            {
                html += "<option value=\"" + podkategorija.ID.ToString() + "\">" + podkategorija.Naziv + "</option>";
            }
            return html;
        }
        public IActionResult PrikazSedmice(int? id, int? planID)
        {
            if (planID != null && id == null)
            {
                id = ajaxService.GetIDOfFirstWeekInPlan(planID);
            }
            AjaxPrikazSedmiceVM model = new AjaxPrikazSedmiceVM
            {
                SedmicaID = id,
                Dani = ajaxService.GetDaniBySedmicaID(id).Select(x => new AjaxPrikazSedmiceVM.Row
                {
                    ID = x.ID,
                    RedniBrojDana = x.RedniBroj,
                    DuzinaTreninga = x.DuzinaTreninga
                }).ToList()
            };
            return PartialView("PrikazSedmice", model);
        }
        public IActionResult PrikazDana(int? id, int? sedmicaID)
        {
            if (sedmicaID != null && id == null)
            {
                id = ajaxService.GetIDOfFirstDayInWeek(sedmicaID);
            }
            AjaxPrikazDanaVM model = new AjaxPrikazDanaVM
            {
                Vjezbe = ajaxService.GetDanVjezbeOrderdByRedniBroj(id).Select(x => new AjaxPrikazDanaVM.Vjezba
                {
                    ID = x.ID,
                    Naziv = x.Vjezba.Naziv,
                    BrojPonavljanja = x.BrojPonavljanja,
                    BrojSetova = x.BrojSetova,
                    RedniBrojVjezbe = x.RedniBrojVjezbe
                }).ToList(),
                DanID = id
            };
            return PartialView("PrikazDana", model);
        }
        public IActionResult ObrisiDanVjezba(int id)
        {
            DanVjezba model = ajaxService.DanVjezbaFind(id);
            if (model == null)
            {
                return PartialView("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            ajaxService.UpdateRedniBroj(model.DanID, model.RedniBrojVjezbe);
            ajaxService.RemoveDanVjezba(model);
            return RedirectToAction("PrikazDana", new { id = model.DanID });
        }
        public IActionResult DodajDanVjezba(int danID)
        {
            AjaxDodajDanVjezbaVM model = new AjaxDodajDanVjezbaVM
            {
                Vjezbe = ajaxService.getVjezbe().Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.ID.ToString()
                }).ToList(),
                DanID = danID,
                RedniBrojevi = ajaxService.GetDanVjezbeOrderdByRedniBroj(danID).Select(x => new SelectListItem
                {
                    Text = x.RedniBrojVjezbe.ToString(),
                    Value = x.RedniBrojVjezbe.ToString(),
                    Selected = false
                }).ToList(),
                RedniBroj = ajaxService.GetMaxRedniBroj(danID)
            };
            model.RedniBrojevi.Add(new SelectListItem
            {
                Text = (model.RedniBrojevi.Count() + 1).ToString(),
                Value = (model.RedniBrojevi.Count() + 1).ToString(),
                Selected = true
            });
            return PartialView("DodajDanVjezba", model);
        }
        public IActionResult SpremiDanVjezba(AjaxDodajDanVjezbaVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Vjezbe = ajaxService.getVjezbe().Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.ID.ToString()
                }).ToList();
                model.RedniBrojevi = ajaxService.GetDanVjezbeOrderdByRedniBroj(model.DanID).Select(x => new SelectListItem
                {
                    Text = x.RedniBrojVjezbe.ToString(),
                    Value = x.RedniBrojVjezbe.ToString(),
                    Selected = false
                }).ToList();
                model.RedniBrojevi.Add(new SelectListItem
                {
                    Text = (model.RedniBrojevi.Count() + 1).ToString(),
                    Value = (model.RedniBrojevi.Count() + 1).ToString(),
                    Selected = true
                });
                return PartialView("DodajDanVjezba", model);
            }
            DanVjezba danVjezba = new DanVjezba
            {
                DanID = model.DanID,
                BrojPonavljanja = model.BrojPonavljanja,
                BrojSetova = model.BrojSetova,
                DuzinaOdmora = model.DuzinaOdmora,
                VjezbaID = model.VjezbaID,
                RedniBrojVjezbe = model.RedniBroj
            };
            ajaxService.IncreaseRedniBroj(model.DanID, model.RedniBroj);
            ajaxService.DodajDanVjezba(danVjezba);

            return RedirectToAction("PrikazDana", new { id = model.DanID });
        }
        public IActionResult UrediDanVjezba(int id)
        {
            DanVjezba danVjezba = ajaxService.DanVjezbaFind(id);
            if (danVjezba == null)
            {
                return PartialView("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AjaxUrediDanVjezbaVM model = new AjaxUrediDanVjezbaVM
            {
                ID = danVjezba.ID,
                DanID = danVjezba.DanID,
                BrojPonavljanja = danVjezba.BrojPonavljanja,
                BrojSetova = danVjezba.BrojSetova,
                DuzinaOdmora = danVjezba.DuzinaOdmora,
                RedniBroj = danVjezba.RedniBrojVjezbe,
                Vjezba = danVjezba.Vjezba.Naziv,
                RedniBrojevi = ajaxService.GetDanVjezbeOrderdByRedniBroj(danVjezba.DanID).Select(y => new SelectListItem
                {
                    Text = y.RedniBrojVjezbe.ToString(),
                    Value = y.RedniBrojVjezbe.ToString(),
                }).ToList()
            };

            return PartialView("UrediDanVjezba", model);
        }
        public IActionResult SpremiUrediDanVjezba(AjaxUrediDanVjezbaVM model)
        {
            if (!ModelState.IsValid)
            {
                model.RedniBrojevi = ajaxService.GetDanVjezbeOrderdByRedniBroj(model.DanID).Select(y => new SelectListItem
                {
                    Text = y.RedniBrojVjezbe.ToString(),
                    Value = y.RedniBrojVjezbe.ToString(),
                }).ToList();
                return PartialView("UrediDanVjezba", model);
            }
            DanVjezba danVjezba = ajaxService.DanVjezbaFind(model.ID);
            danVjezba.BrojSetova = model.BrojSetova;
            danVjezba.BrojPonavljanja = model.BrojPonavljanja;
            danVjezba.DuzinaOdmora = model.DuzinaOdmora;
            ajaxService.ChangeRedniBroj(model.DanID, danVjezba.RedniBrojVjezbe, model.RedniBroj);
            danVjezba.RedniBrojVjezbe = model.RedniBroj;
            ajaxService.UpdateDanVjezba(danVjezba);
            return RedirectToAction("PrikazDana", new { id = model.DanID });
        }
        public IActionResult PrisutstvoClana(string cardNumber)
        {
            PosjecenostClana posjecenostClana = ajaxService.GetPosjecenostClanaByBrojKartice(cardNumber);
            Korisnik clan = ajaxService.GetClanByBrojKartice(cardNumber);
            if (clan == null)
            {
                return RedirectToAction("PrikazPrisutnihClanova");
            }
            if (posjecenostClana == null)
            {
                posjecenostClana = new PosjecenostClana
                {
                    Datum = DateTime.Today,
                    VrijemeDolaska = DateTime.Now,
                    ClanID = clan.ID
                };

                ajaxService.DodajPosjecenostClana(posjecenostClana);
            }
            else
            {
                posjecenostClana.VrijemeOdlaska = DateTime.Now;
                ajaxService.UpdatePosjecenostClana(posjecenostClana);
            }
            return RedirectToAction("PrikazPrisutnihClanova");
        }
        public IActionResult PrikazPrisutnihClanova()
        {
            AdministracijaPrikazPrisutnihClanovaVM model = new AdministracijaPrikazPrisutnihClanovaVM
            {
                Clanovi = ajaxService.GetPosjecenostClana().Where(x => x.VrijemeOdlaska == null).Select(x => new AdministracijaPrikazPrisutnihClanovaVM.Row
                {
                    ID = x.ClanID,
                    Ime = x.Clan.Ime + " " + x.Clan.Prezime,
                    VrijemeDolaska = x.VrijemeDolaska.ToString("hh:mm"),
                    Slika = x.Clan.Slika,
                    AktivnaClanarina = clanService.AktivnaClanarina(x.Clan.ID) == null ? "ne" : "da"
                }).ToList()
            };
            return PartialView("PrikazPrisutnihClanova", model);
        }
        public IActionResult UrediPlanIProgram(int id)
        {
            PlanIProgram planIProgram = planIProgramService.PlanIProgramFind(id);
            AjaxUrediPiPVM model = new AjaxUrediPiPVM
            {
                ID = planIProgram.ID,
                Kategorija = planIProgram.Kategorija.Naziv,
                Naziv = planIProgram.Naziv,
                Opis = planIProgram.Opis
            };
            return PartialView("UrediPlanIProgram", model);
        }
        public IActionResult PrikazSkladiste(int id)
        {
            AjaxPrikazSkladistaVM model = new AjaxPrikazSkladistaVM
            {
                skladiste = webShopService.GetSkladista(id).Select(x => new AjaxPrikazSkladistaVM.Row
                {
                    ID = x.ID,
                    Kolicina = x.Kolicina,
                    Velicina = x.Velicina
                }).ToList(),
                StavkaID = id
            };
            return PartialView("PrikazSkladiste", model);
        }
        public IActionResult DodajSkladiste(int id)
        {
            AjaxDodajSkladisteVM model = new AjaxDodajSkladisteVM
            {
                StavkaID = id
            };
            return PartialView("DodajSkladiste", model);
        }
        public IActionResult SpremiSkladiste(AjaxDodajSkladisteVM model)
        {
            Skladiste skladiste;
            if (model.SkladisteID == null)
            {
                skladiste = new Skladiste();
                skladiste.StavkaID = model.StavkaID;
                webShopService.DodajSkladiste(skladiste);
            }
            else
            {
                int id = model.SkladisteID ?? 0;
                skladiste = webShopService.SkladisteFind(id);
            }

            skladiste.Kolicina = model.Kolicina;
            skladiste.Velicina = model.Velicina;
            webShopService.UpdateSkladiste(skladiste);
            return RedirectToAction("PrikazSkladiste", new { id = model.StavkaID });
        }
        public IActionResult UrediSkladiste(int id)
        {
            Skladiste skladiste = webShopService.SkladisteFind(id);
            AjaxDodajSkladisteVM model = new AjaxDodajSkladisteVM
            {
                SkladisteID = skladiste.ID,
                StavkaID = skladiste.StavkaID,
                Kolicina = skladiste.Kolicina,
                Velicina = skladiste.Velicina
            };
            return PartialView("DodajSkladiste", model);
        }
        public bool userTypeCheck()
        {
            KorisnickiNalog korisnickiNalog = HttpContext.GetLogiraniKorisnik();
            if (korisnickiNalog.Tip == "admin")
            {
                return true;
            }
            return false;
        }
        public IActionResult PrikazPodkategorija(int id)
        {
            AjaxPrikazPodkategorijaVM model = new AjaxPrikazPodkategorijaVM
            {
                Podkategorije = webShopService.GetPodkategorije().Where(x => x.Obrisan == false).Where(x => x.KategorijaID == id).Select(x => new AjaxPrikazPodkategorijaVM.Row
                {
                    ID = x.ID,
                    Naziv = x.Naziv
                }).ToList(),
                KategorijaID = id
            };
            return PartialView("PrikazPodkategorija", model);
        }
        public IActionResult ObrisiPodkategoriju(int id)
        {
            Podkategorija podkategorija = webShopService.GetPodkategorije().Where(x => x.ID == id).SingleOrDefault();
            if(podkategorija == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            podkategorija.Obrisan = true;
            webShopService.UpdatePodkategorija(podkategorija);
            return RedirectToAction("PrikazPodkategorija", new { id = podkategorija.KategorijaID });
        }
        public IActionResult DodajPodkategoriju(int id)
        {
            Kategorija kategorija = webShopService.GetKategorijaByID(id);
            AjaxDodajPodkategorijuVM model = new AjaxDodajPodkategorijuVM
            {
                Kategorija = kategorija.Naziv,
                KategorijaID = kategorija.ID
            };
            return PartialView("DodajPodkategoriju", model);
        }
        public IActionResult SpremiPodkategoriju(AjaxDodajPodkategorijuVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("DodajPodkategoriju", model);
            }
            Podkategorija podkategorija = new Podkategorija
            {
                KategorijaID = model.KategorijaID,
                Naziv = model.Naziv
            };
            webShopService.DodajPodkategoriju(podkategorija);
            return RedirectToAction("PrikazPodkategorija", new { id = podkategorija.KategorijaID });
        }
        public IActionResult ObrisiKategoriju(int id)
        {
            Kategorija kategorija = webShopService.GetKategorijaByID(id);
            if(kategorija == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            List<Podkategorija> podkategorije = webShopService.GetPodkategorije().Where(x => x.KategorijaID == id).ToList();
            foreach(var x in podkategorije)
            {
                x.Obrisan = true;
                webShopService.UpdatePodkategorija(x);
            }
            kategorija.Obrisan = true;
            webShopService.UpdateKategorija(kategorija);
            return RedirectToAction("PrikazKategorija", "AdministracijaWebShop");
        }
    }
}