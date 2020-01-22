using FitnessCentar.data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessCentar.service.Interfaces
{
    public interface IWebShopService
    {
        IEnumerable<Kategorija> GetKategorije();
        void DodajKategoriju(Kategorija kategorija);
        IEnumerable<Podkategorija> GetPodkategorije();
        List<SelectListItem> GetKategorijeSelectListItem();
        void DodajPodkategoriju(Podkategorija podkategorija);
        void DodajStavku(Stavka stavka);
        IEnumerable<Stavka> GetStavke();
        Stavka StavkaFind(int id);
        void UpdateStavka(Stavka stavka);
        IEnumerable<Narudzba> GetNarudzbe();
        int BrojStavki(int narudzbaID);
        Narudzba NarudzbaFind(int id);
        IEnumerable<NarudzbaStavke> GetNarudzbaStavke(int naruzbaID);
        void DodajRacun(Racun racun);
        void UpdateNarudzba(Narudzba narudzba);
        IEnumerable<Racun> GetRacune();
        IEnumerable<Skladiste> GetSkladista(int id);
        void DodajSkladiste(Skladiste skladiste);
        Skladiste SkladisteFind(int id);
        void UpdateSkladiste(Skladiste skladiste);
        Narudzba NarudzbaByRacun(int racunID);
        Kategorija GetKategorijaByID(int id);
        void UpdatePodkategorija(Podkategorija podkategorija);
        void UpdateKategorija(Kategorija kategorija);
    }
}
