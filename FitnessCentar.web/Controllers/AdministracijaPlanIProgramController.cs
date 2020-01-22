using System.Collections.Generic;
using System.IO;
using System.Linq;
using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Helper;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using FitnessCentar.web.ViewModels.AjaxVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessCentar.web.Controllers
{
    [Autorizacija(admin: true, clan: false, zaposlenik: true)]
    public class AdministracijaPlanIProgramController : Controller
    {
        IPlanIProgramService service;
        private readonly IHostingEnvironment _environment;
        public AdministracijaPlanIProgramController(IPlanIProgramService _service, IHostingEnvironment hostingEnvironment)
        {
            service = _service;
            _environment = hostingEnvironment;
        }
        public IActionResult PlanIProgramLista()
        {
            AdministracijaPlanIProgramVM model = new AdministracijaPlanIProgramVM
            {
                Planovi = service.getPlanovi().Where(x => x.Obrisan == false).Select(x => new AdministracijaPlanIProgramVM.Row
                {
                    ID = x.ID,
                    Naziv = x.Naziv,
                    Kategorija = x.Kategorija.Naziv,
                    Opis = x.Opis.Length > 500 ? x.Opis.Substring(0, 500) + "..." : x.Opis
                }).ToList()
            };
            if (TempData["obrisanPlan"] != null)
            {
                ViewBag.ObrisanPlan = TempData["obrisanPlan"].ToString();
            }
            return View("PlanIProgramLista", model);
        }
        public IActionResult DodajPlanIProgram()
        {
            AdministracijaDodajPlanIProgramVM model = new AdministracijaDodajPlanIProgramVM
            {
                Kategorije = service.getKategorije().Where(x => x.Obrisan == false).Select(k => new SelectListItem
                {
                    Value = k.ID.ToString(),
                    Text = k.Naziv
                }).ToList(),
            };
            return View("DodajPlanIProgram", model);
        }
        public IActionResult SpremiPlanIProgram(AdministracijaDodajPlanIProgramVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Kategorije = service.getKategorije().Select(k => new SelectListItem
                {
                    Value = k.ID.ToString(),
                    Text = k.Naziv
                }).ToList();
                return View("DodajPlanIProgram", model);
            }

            PlanIProgram planIProgram = new PlanIProgram
            {
                KategorijaID = model.KategorijaId,
                Naziv = model.Naziv,
                Opis = model.Opis
            };
            service.DodajPlanIProgram(planIProgram);

            for (int i = 1; i <= model.BrojSedmica; i++)
            {
                Sedmica sedmica = new Sedmica
                {
                    PlanIProgramID = planIProgram.ID,
                    RedniBroj = i
                };
                service.DodajSedmicu(sedmica);
                for (int j = 1; j <= 7; j++)
                {
                    Dan dan = new Dan
                    {
                        SedmicaID = sedmica.ID,
                        RedniBroj = j
                    };
                    service.DodajDan(dan);
                }
            }
            return RedirectToAction("PrikazPlanIProgram", new { planID = planIProgram.ID });
        }
        public IActionResult PrikazPlanIProgram(int planID)
        {
            PlanIProgram planIProgram = service.PlanIProgramFind(planID);
            if (planIProgram == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaPrikazPlanIProgramVM model = new AdministracijaPrikazPlanIProgramVM
            {
                ID = planID,
                Naziv = planIProgram.Naziv,
                Kategorija = planIProgram.Kategorija.Naziv,
                Opis = planIProgram.Opis,
                Sedmice = service.GetSedmiceOrderdByRedniBroj(planID).Select(x => new AdministracijaPrikazPlanIProgramVM.Sedmica
                {
                    RedniBroj = x.RedniBroj,
                    ID = x.ID,
                    Dani = service.GetDaniOrderedByRedniBroj(x.ID).Select(y => new AdministracijaPrikazPlanIProgramVM.Dan
                    {
                        ID = y.ID,
                        RedniBroj = y.RedniBroj
                    }).ToList()
                }).ToList()
            };
            return View("PrikazPlanIProgram", model);
        }
        public IActionResult ObrisiPlanIProgram(int id)
        {
            PlanIProgram planIProgram = service.PlanIProgramFind(id);
            if (planIProgram == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            planIProgram.Obrisan = true;
            TempData["obrisanPlan"] = planIProgram.Naziv;
            service.UpdatePlanIProgram(planIProgram);
            return RedirectToAction("PlanIProgramLista");
        }
        public IActionResult ObrisiSedmicu(int id)
        {
            Sedmica sedmica = service.SedmicaFind(id);
            if (sedmica == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            List<DanVjezba> danVjezbas = service.GetDanVjezbe(sedmica.ID).ToList();
            List<Dan> dans = service.GetDan(sedmica.ID).ToList();
            danVjezbas.ForEach(x => service.ObrisiDanVjezba(x));
            dans.ForEach(x => service.ObrisiDan(x));
            service.ObrisiSedmicu(sedmica);
            return RedirectToAction("PrikazPlanIProgram", new { planID = sedmica.PlanIProgramID });

        }
        public IActionResult DodajVjezbu()
        {
            AdministracijaDodajVjezbuVM model = new AdministracijaDodajVjezbuVM();
            return View("DodajVjezbu", model);
        }
        public IActionResult EditVjezbu(int id)
        {
            Vjezba vjezba = service.VjezbaFind(id);
            if (vjezba == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaEditVjezbuVM model = new AdministracijaEditVjezbuVM
            {
                ID = vjezba.ID,
                Naziv = vjezba.Naziv,
                Opis = vjezba.Opis
            };
            return View("UrediVjezbu", model);
        }
        public IActionResult SpremiEditVjezbu(AdministracijaEditVjezbuVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("UrediVjezbu", model);
            }
            Vjezba vjezba = service.VjezbaFind(model.ID);
            vjezba.Opis = model.Opis;
            if (model.Slika != null)
            {
                vjezba.Slika = model.Naziv + Path.GetExtension(model.Slika.FileName);
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images/Vjezbe/");
                string filePath = Path.Combine(uploadsFolder, vjezba.Slika);
                model.Slika.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            service.UpdateVjezbu(vjezba);
            return RedirectToAction("PrikazVjezbi");
        }
        public IActionResult SpremiVjezbu(AdministracijaDodajVjezbuVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("DodajVjezbu", model);
            }

            Vjezba vjezba = new Vjezba
            {
                Naziv = model.Naziv,
                Opis = model.Opis
            };           
            if (model.Slika != null)
            {
                vjezba.Slika = model.Naziv + Path.GetExtension(model.Slika.FileName);
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images/Vjezbe/");
                string filePath = Path.Combine(uploadsFolder, vjezba.Slika);
                model.Slika.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            TempData["novaVjezba"] = vjezba.Naziv;
            service.DodajVjezbu(vjezba);
            return RedirectToAction("PrikazVjezbi");
        }
        public IActionResult PrikazVjezbi()
        {
            AdministracijaPrikazVjezbiVM model = new AdministracijaPrikazVjezbiVM
            {
                Vjezbe = service.GetVjezbe().Select(v => new AdministracijaPrikazVjezbiVM.Vjezba
                {
                    ID = v.ID,
                    Naziv = v.Naziv,
                    Opis = v.Opis.Length > 500 ? v.Opis.Substring(0, 500) + "..." : v.Opis,
                    Slika = v.Slika
                }).ToList()
            };
            if(TempData["novaVjezba"] != null)
            {
                ViewBag.NovaVjezba = TempData["novaVjezba"].ToString();
            }
            else if(TempData["obrisanaVjezba"] != null)
            {
                ViewBag.ObrisanaVjezba = TempData["obrisanaVjezba"].ToString();
            }
            return View("PrikazVjezbi", model);
        }
        public IActionResult PrikazKategorija()
        {
            AdministracijaPrikazKategorijaPlanIProgramVM model = new AdministracijaPrikazKategorijaPlanIProgramVM
            {
                kategorije = service.getKategorije().Where(x => x.Obrisan == false).Select(x => new AdministracijaPrikazKategorijaPlanIProgramVM.Row
                {
                    ID = x.ID,
                    Naziv = x.Naziv
                }).ToList()
            };
            if(TempData["novaKategorijaPlan"] != null)
            {
                ViewBag.NovaKategorijaPlan = TempData["novaKategorijaPlan"].ToString();
            }
            else if (TempData["obrisanaKategorijaPlan"] != null)
            {
                ViewBag.ObrisanaKategorijaPlan = TempData["obrisanaKategorijaPlan"].ToString();
            }
            return View("PrikazKategorija", model);
        }
        public IActionResult DodajKategoriju()
        {
            AdministracijaDodajKategorijuPIPVM model = new AdministracijaDodajKategorijuPIPVM();
            return View("DodajKategoriju", model);
        }
        public IActionResult SpremiKategoriju(AdministracijaDodajKategorijuPIPVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("DodajKategoriju", model);
            }
            if (model.ID == null)
            {
                KategorijaPlanIProgram kategorija = new KategorijaPlanIProgram
                {
                    Naziv = model.Naziv
                };
                service.DodajKategoriju(kategorija);
                TempData["novaKategorijaPlan"] = kategorija.Naziv;
            }
            else
            {
                int _id = model.ID ?? 0;
                KategorijaPlanIProgram kategorija = service.KategorijaPlanIProgramFind(_id);
                kategorija.Naziv = model.Naziv;
                service.UpdateKategoriju(kategorija);
            }
            return RedirectToAction("PrikazKategorija");
        }
        public IActionResult UrediKategoriju(int id)
        {
            KategorijaPlanIProgram kategorija = service.KategorijaPlanIProgramFind(id);
            if (kategorija == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            AdministracijaDodajKategorijuPIPVM model = new AdministracijaDodajKategorijuPIPVM
            {
                Naziv = kategorija.Naziv,
                ID = kategorija.ID
            };
            return View("DodajKategoriju", model);
        }
        public IActionResult ObrisiKategoriju(int id)
        {
            KategorijaPlanIProgram kategorija = service.KategorijaPlanIProgramFind(id);
            if (kategorija == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            kategorija.Obrisan = true;
            service.UpdateKategoriju(kategorija);
            TempData["obrisanaKategorijaPlan"] = kategorija.Naziv;
            return RedirectToAction("PrikazKategorija");
        }
        public IActionResult ObrisiVjezbu(int id)
        {
            Vjezba vjezba = service.VjezbaFind(id);
            if (vjezba == null)
            {
                return View("~/Views/Home/NotFoundAdministracija.cshtml");
            }
            service.ObrisiVjezbu(vjezba);
            TempData["obrisanaVjezba"] = vjezba.Naziv;
            return RedirectToAction("PrikazVjezbi");
        }
        public IActionResult SpremiEditPlanIProgram(AjaxUrediPiPVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Ajax/UrediPlanIProgram.cshtml",model);
            }
            PlanIProgram planIProgram = service.PlanIProgramFind(model.ID);
            planIProgram.Opis = model.Opis;
            service.UpdatePlanIProgram(planIProgram);
            return RedirectToAction("PrikazPlanIProgram", new { planID = model.ID });
        }
    }
}