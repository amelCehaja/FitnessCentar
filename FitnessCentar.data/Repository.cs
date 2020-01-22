using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FitnessCentar.data.EF;
using FitnessCentar.data.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCentar.data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        MyContext db;
        DbSet<TEntity> dbSet;
        public Repository(MyContext myContext)
        {
            db = myContext;
            dbSet = db.Set<TEntity>();
        }
        public void Add(TEntity obj)
        {
            dbSet.Add(obj);
            db.SaveChanges();
        }

        public IEnumerable<Adresa> GetAdresa()
        {
            IEnumerable<Adresa> adrese = db.Adresa.Include(x => x.Korisnik);
            return adrese;
        }

        public IEnumerable<Clanarina> GetClanarina()
        {
            IEnumerable<Clanarina> clanarine = db.Clanarina.Include(x => x.TipClanarine).Include(x => x.Clan);
            return clanarine;
        }

        public IEnumerable<Dan> GetDan()
        {
            IEnumerable<Dan> dani = db.Dan.Include(x => x.Sedmica).ThenInclude(x => x.PlanIProgram).ThenInclude(x => x.Kategorija);
            return dani;
        }

        public IEnumerable<DanVjezba> GetDanVjezba()
        {
            IEnumerable<DanVjezba> danVjezbe = db.DanVjezba.Include(x => x.Dan).Include(x => x.Vjezba);
            return danVjezbe;
        }

        public IEnumerable<Kategorija> GetKategorija()
        {
            IEnumerable<Kategorija> kategorije = db.Kategorija;
            return kategorije;
        }

        public IEnumerable<KategorijaPlanIProgram> GetKategorijaPlanIProgram()
        {
            IEnumerable<KategorijaPlanIProgram> kategorije = db.KategorijaPlanIProgram;
            return kategorije;
        }

        public IEnumerable<Korisnik> GetClan()
        {
            return db.Korisnik.Include(x => x.KorisnickiNalog).Where(x => x.KorisnickiNalog.Tip == "clan");
        }

        public IEnumerable<Korisnik> GetZaposlenik()
        {
            return db.Korisnik.Include(x => x.KorisnickiNalog).Where(x => x.KorisnickiNalog.Tip == "zaposlenik");
        }

        public IEnumerable<MjerenjeNapredka> GetMjerenjeNapredka()
        {
            IEnumerable<MjerenjeNapredka> mjerenjeNapredka = db.MjerenjeNapredka.Include(x => x.Clan);
            return mjerenjeNapredka;
        }

        public IEnumerable<NacinPlacanja> GetNacinPlacanja()
        {
            IEnumerable<NacinPlacanja> nacinPlacanja = db.NacinPlacanja;
            return nacinPlacanja;
        }

        public IEnumerable<Narudzba> GetNarudzba()
        {
            IEnumerable<Narudzba> narudzbe = db.Narudzba.Include(x => x.Korisnik).Include(x => x.NacinPlacanja).Include(x => x.Racun).Include(x => x.Adresa);
            return narudzbe;
        }

        public IEnumerable<NarudzbaStavke> GetNarudzbaStavke()
        {
            IEnumerable<NarudzbaStavke> narudzbaStavke = db.NarudzbaStavke.Include(x => x.Narudzba).Include(x => x.Stavka).ThenInclude(x => x.Podkategorija).ThenInclude(x => x.Kategorija).Include(x => x.Velicina);
            return narudzbaStavke;
        }

        public IEnumerable<PlanIProgram> GetPlanIProgram()
        {
            IEnumerable<PlanIProgram> planIProgram = db.PlanIProgram.Where(x => x.Obrisan == false).Include(x => x.Kategorija);
            return planIProgram;
        }

        public IEnumerable<Podkategorija> GetPodkategorija()
        {
            IEnumerable<Podkategorija> podkategorija = db.Podkategorija.Include(x => x.Kategorija);
            return podkategorija;
        }

        public IEnumerable<PosjecenostClana> GetPosjecenostClana()
        {
            IEnumerable<PosjecenostClana> posjecenostClana = db.PosjecenostClana.Include(x => x.Clan);
            return posjecenostClana;
        }

        public IEnumerable<Racun> GetRacun()
        {
            IEnumerable<Racun> racun = db.Racun.Include(x => x.Zaposlenik);
            return racun;
        }

        public IEnumerable<Sedmica> GetSedmica()
        {
            IEnumerable<Sedmica> sedmica = db.Sedmica.Include(x => x.PlanIProgram).ThenInclude(x => x.Kategorija);
            return sedmica;
        }

        public IEnumerable<Skladiste> GetSkladiste()
        {
            return db.Skladiste.Include(x => x.Stavka);
        }

        public IEnumerable<Stavka> GetStavka()
        {
            IEnumerable<Stavka> stavka = db.Stavka.Include(x => x.Podkategorija).ThenInclude(x => x.Kategorija);
            return stavka;
        }

        public IEnumerable<TipClanarine> GetTipClanarine()
        {
            IEnumerable<TipClanarine> tipClanarine = db.TipClanarine;
            return tipClanarine;
        }

        public IEnumerable<Vjezba> GetVjezbas()
        {
            IEnumerable<Vjezba> vjezba = db.Vjezba;
            return vjezba;
        }

        public void Remove(TEntity obj)
        {
            dbSet.Remove(obj);
            db.SaveChanges();
        }

        public void Update(TEntity obj)
        {
            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
