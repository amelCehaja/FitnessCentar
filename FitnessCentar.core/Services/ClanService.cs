using FitnessCentar.data;
using FitnessCentar.data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessCentar.service.Interfaces;

namespace FitnessCentar.service.Services
{
    public class ClanService : IClanService
    {
        IRepository<TipClanarine> tipClanarineRepository;
        IRepository<Korisnik> clanRepository;
        IRepository<Clanarina> clanarinaRepostiory;
        IRepository<KorisnickiNalog> korisnickiNalogRepository;
        public ClanService(IRepository<TipClanarine> _tipClanarineRepository, IRepository<Korisnik> _clanRepository, IRepository<Clanarina> _clanarinaRepository, IRepository<KorisnickiNalog> _korisnickiNalogRepository)
        {
            tipClanarineRepository = _tipClanarineRepository;
            clanRepository = _clanRepository;
            clanarinaRepostiory = _clanarinaRepository;
            korisnickiNalogRepository = _korisnickiNalogRepository;
        }
        public void DodajClana(Korisnik clan)
        {
            clanRepository.Add(clan);
        }
        public void DodajClanarinu(Clanarina clanarina)
        {
            clanarinaRepostiory.Add(clanarina);
        }
        public List<SelectListItem> GetTipoviClanarineSelectList()
        {
            List<SelectListItem> tipoviClanarine = tipClanarineRepository.GetTipClanarine().Where(x => x.Obrisan == false).Select(x => new SelectListItem
            {
                Text = x.Naziv + " - " + x.Cijena + " KM - " + x.VrijemeTrajanja + " dana",
                Value = x.ID.ToString()
            }).ToList();
            return tipoviClanarine;
        }
        public Korisnik ClanFind(int id)
        {
            Korisnik clan = clanRepository.GetClan().Where(x => x.ID == id && x.KorisnickiNalog.Tip=="clan").SingleOrDefault();
            return clan;
        }
        public int BrojAktivnihClanarina()
        {
            return clanRepository.GetClanarina().Where(x => x.DatumDodavanja <= DateTime.Today && x.DatumIsteka >= DateTime.Today).Select(x => x.ClanID).Distinct().Count();
        }
        public int UkupanBrojClanova()
        {
            return clanRepository.GetClan().Count();
        }
        public List<SelectListItem> GetTipoviClanarine()
        {
            return tipClanarineRepository.GetTipClanarine().Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.ID.ToString()
            }).ToList();
        }
        public void ClanUpdate(Korisnik clan)
        {
            clanRepository.Update(clan);
        }
        public IEnumerable<TipClanarine> GetAllTipoviClanarine()
        {
            return tipClanarineRepository.GetTipClanarine();
        }
        public TipClanarine TipClanarineFind(int id)
        {
            return tipClanarineRepository.GetTipClanarine().Where(x => x.ID == id).SingleOrDefault();
        }
        public void DodajTipClanarine(TipClanarine tipClanarine)
        {
            tipClanarineRepository.Add(tipClanarine);
        }
        public void UpdateTipClanarine(TipClanarine tipClanarine)
        {
            tipClanarineRepository.Update(tipClanarine);
        }
        public Clanarina AktivnaClanarina(int clanID)
        {
            return clanarinaRepostiory.GetClanarina()
                .Where(x => x.ClanID == clanID)
                .Where(p => p.DatumDodavanja.Date <= DateTime.Today)
                .Where(k => k.DatumIsteka.Date >= DateTime.Today)
                .FirstOrDefault();
        }
        public string DatumPrveClanarine(int clanID)
        {
            return clanarinaRepostiory.GetClanarina()
                .Where(c => c.ClanID == clanID)
                .OrderBy(o => o.DatumDodavanja)
                .Select(x => x.DatumDodavanja)
                .FirstOrDefault()
                .ToString();
        }
        public IEnumerable<Clanarina> GetClanarineByClanID(int id)
        {
            return clanarinaRepostiory.GetClanarina().Where(x => x.ClanID == id);
        }
        public bool IsUsernameUnique(string username) {
            Korisnik clan = clanarinaRepostiory.GetClan().Where(x => x.KorisnickiNalog.KorisnickoIme == username).SingleOrDefault();
            if (clan == null)
            {
                return true;
            }
            return false;
        }
        public void DodajKorisnickiNalog(KorisnickiNalog korisnickiNalog)
        {
            korisnickiNalogRepository.Add(korisnickiNalog);
        }
    }
}
