using FitnessCentar.data;
using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitnessCentar.service.Services
{
    public class WebShopService : IWebShopService
    {
        IRepository<Kategorija> kategorijaRepository;
        IRepository<Podkategorija> podkategorijaRepository;
        IRepository<Stavka> stavkaRepository;
        IRepository<Narudzba> narudzbaRepository;
        IRepository<Racun> racunRepository;
        IRepository<Skladiste> skladisteRepository;
        public WebShopService(IRepository<Kategorija> _kategorijaRepository, IRepository<Podkategorija> _podkategorijaRepository, IRepository<Stavka>_stavkaRepository, IRepository<Narudzba> _narudzbaRepository, IRepository<Racun> _racunRepository, IRepository<Skladiste> _skladisteRepository)
        {
            kategorijaRepository = _kategorijaRepository;
            podkategorijaRepository = _podkategorijaRepository;
            stavkaRepository = _stavkaRepository;
            narudzbaRepository = _narudzbaRepository;
            racunRepository = _racunRepository;
            skladisteRepository = _skladisteRepository;
        }
        public IEnumerable<Kategorija> GetKategorije()
        {
            return kategorijaRepository.GetKategorija();
        }
        public void DodajKategoriju(Kategorija kategorija)
        {
            kategorijaRepository.Add(kategorija);
        }
        public IEnumerable<Podkategorija> GetPodkategorije()
        {
            return kategorijaRepository.GetPodkategorija();
        }
        public List<SelectListItem> GetKategorijeSelectListItem()
        {
            return kategorijaRepository.GetKategorija().Where(x => x.Obrisan == false).Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.ID.ToString()
            }).ToList();
        }
        public void DodajPodkategoriju(Podkategorija podkategorija)
        {
            podkategorijaRepository.Add(podkategorija);
        }
        public void DodajStavku(Stavka stavka)
        {
            stavkaRepository.Add(stavka);
        }
        public IEnumerable<Stavka> GetStavke()
        {
            return stavkaRepository.GetStavka();
        }
        public Stavka StavkaFind(int id)
        {
            return stavkaRepository.GetStavka().Where(x => x.ID == id).FirstOrDefault();
        }
        public void UpdateStavka(Stavka stavka)
        {
            stavkaRepository.Update(stavka);
        }
        public IEnumerable<Narudzba> GetNarudzbe()
        {
            return narudzbaRepository.GetNarudzba().Where(x => x.RacunID == null);
        }
        public int BrojStavki(int narudzbaID)
        {
            return narudzbaRepository.GetNarudzbaStavke().Where(x => x.NarudzbaID == narudzbaID).Sum(x => x.Kolicina);
        }
        public Narudzba NarudzbaFind(int id)
        {
            return narudzbaRepository.GetNarudzba().Where(x => x.ID == id).SingleOrDefault();
        }
        public IEnumerable<NarudzbaStavke> GetNarudzbaStavke(int naruzbaID)
        {
            return narudzbaRepository.GetNarudzbaStavke().Where(x => x.NarudzbaID == naruzbaID);
        }
        public void DodajRacun(Racun racun)
        {
            racunRepository.Add(racun);
        }
        public void UpdateNarudzba(Narudzba narudzba)
        {
            narudzbaRepository.Update(narudzba);
        }
        public IEnumerable<Racun> GetRacune()
        {
            return racunRepository.GetRacun();
        }
        public IEnumerable<Skladiste> GetSkladista(int id)
        {
            return skladisteRepository.GetSkladiste().Where(x => x.StavkaID == id);
        }
        public void DodajSkladiste(Skladiste skladiste)
        {
            skladisteRepository.Add(skladiste);
        }
        public Skladiste SkladisteFind(int id)
        {
            return skladisteRepository.GetSkladiste().Where(x => x.ID == id).SingleOrDefault();
        }
        public void UpdateSkladiste(Skladiste skladiste)
        {
            skladisteRepository.Update(skladiste);
        }
        public Narudzba NarudzbaByRacun(int racunID)
        {
            return narudzbaRepository.GetNarudzba().Where(x => x.RacunID == racunID).SingleOrDefault();
        }
        public Kategorija GetKategorijaByID(int id)
        {
            return kategorijaRepository.GetKategorija().Where(x => x.ID == id).SingleOrDefault();
        }
        public void UpdatePodkategorija(Podkategorija podkategorija)
        {
            podkategorijaRepository.Update(podkategorija);
        }
        public void UpdateKategorija(Kategorija kategorija)
        {
            kategorijaRepository.Update(kategorija);
        }
    }
}
