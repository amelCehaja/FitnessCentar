using FitnessCentar.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessCentar.data
{
    public interface IRepository<TEntity>
    {
        void Add(TEntity obj);
        void Remove(TEntity obj);
        void Update(TEntity obj);
        IEnumerable<Adresa> GetAdresa();
        IEnumerable<Clanarina> GetClanarina();
        IEnumerable<Dan> GetDan();
        IEnumerable<DanVjezba> GetDanVjezba();
        IEnumerable<Kategorija> GetKategorija();
        IEnumerable<KategorijaPlanIProgram> GetKategorijaPlanIProgram();
        IEnumerable<MjerenjeNapredka> GetMjerenjeNapredka();
        IEnumerable<NacinPlacanja> GetNacinPlacanja();
        IEnumerable<Narudzba> GetNarudzba();
        IEnumerable<NarudzbaStavke> GetNarudzbaStavke();
        IEnumerable<PlanIProgram> GetPlanIProgram();
        IEnumerable<Podkategorija> GetPodkategorija();
        IEnumerable<PosjecenostClana> GetPosjecenostClana();
        IEnumerable<Racun> GetRacun();
        IEnumerable<Sedmica> GetSedmica();
        IEnumerable<Skladiste> GetSkladiste();
        IEnumerable<Stavka> GetStavka();
        IEnumerable<TipClanarine> GetTipClanarine();
        IEnumerable<Vjezba> GetVjezbas();
        IEnumerable<Korisnik> GetClan();
        IEnumerable<Korisnik> GetZaposlenik();
    }
}
