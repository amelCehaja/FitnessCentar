using FitnessCentar.data;
using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitnessCentar.service.Services
{
    public class AjaxService : IAjaxService
    {
        IRepository<TipClanarine> tipClanarineRepository;
        IRepository<DanVjezba> danVjezbaRepository;
        IRepository<PosjecenostClana> posjecenostClanaRepository;
        public AjaxService(IRepository<TipClanarine> _tipClanarineRepository, IRepository<DanVjezba> _danVjezbaRepository, IRepository<PosjecenostClana> _posjecenostClanaRepository)
        {
            tipClanarineRepository = _tipClanarineRepository;
            danVjezbaRepository = _danVjezbaRepository;
            posjecenostClanaRepository = _posjecenostClanaRepository;
        }
        public int TrajanjeTipaClanarine(int id)
        {
            return tipClanarineRepository.GetTipClanarine().Where(x => x.ID == id).Select(x => x.VrijemeTrajanja).SingleOrDefault();
        }
        public List<Clanarina> GetClanarineByStatus(string status)
        {
            if (status == "sve")
            {
                return tipClanarineRepository.GetClanarina().ToList();
            }
            else if (status == "da")
            {
                return tipClanarineRepository.GetClanarina().Where(c => c.DatumDodavanja <= DateTime.Today && c.DatumIsteka >= DateTime.Today).ToList();
            }
            else
            {
                return tipClanarineRepository.GetClanarina().Where(c => c.DatumDodavanja > DateTime.Today || c.DatumIsteka < DateTime.Today).ToList();

            }
        }
        public IEnumerable<Korisnik> PrikazClanovaPoImenu(string imePrezime)
        {
            IEnumerable<Korisnik> sviClanovi = tipClanarineRepository.GetClan();
            IEnumerable<Korisnik> clanovi;
            if (imePrezime != null)
            {
                imePrezime = imePrezime.ToLower();
                string[] _imePrezime = imePrezime.Split(" ");          
                clanovi = sviClanovi.Where(k => k.Ime.ToLower().Contains(_imePrezime[0]) == true || k.Prezime.ToLower().Contains(_imePrezime[0]) == true);                
                if(_imePrezime.Length > 1)
                {
                    clanovi = clanovi.Where(k => k.Ime.ToLower().Contains(_imePrezime[1]) == true || k.Prezime.ToLower().Contains(_imePrezime[1]) == true);
                }
            }
            else
            {
                clanovi = sviClanovi;
            }
            return clanovi;
        }
        public IEnumerable<Podkategorija> GetPodkategorijeByKategorijaID(int kategorijaID)
        {
            return tipClanarineRepository.GetPodkategorija().Where(x => x.KategorijaID == kategorijaID && x.Obrisan == false);
        }
        public int GetIDOfFirstWeekInPlan(int? planID)
        {
            return tipClanarineRepository.GetSedmica().Where(x => x.PlanIProgramID == planID).OrderBy(x => x.RedniBroj).Select(x => x.ID).FirstOrDefault();
        }
        public IEnumerable<Dan> GetDaniBySedmicaID(int? sedmicaID)
        {
            return tipClanarineRepository.GetDan().Where(x => x.SedmicaID == sedmicaID);
        }
        public int GetIDOfFirstDayInWeek(int? sedmicaID)
        {
            return tipClanarineRepository.GetDan().Where(x => x.SedmicaID == sedmicaID).OrderBy(x => x.RedniBroj).Select(x => x.ID).FirstOrDefault();
        }
        public IEnumerable<DanVjezba> GetDanVjezbeOrderdByRedniBroj(int? danID)
        {
            return tipClanarineRepository.GetDanVjezba().Where(x => x.DanID == danID).OrderBy(x => x.RedniBrojVjezbe);
        }
        public DanVjezba DanVjezbaFind(int id)
        {
            return danVjezbaRepository.GetDanVjezba().Where(x => x.ID == id).SingleOrDefault();
        }
        public void UpdateRedniBroj(int danID, int redniBroj)
        {
            IEnumerable<DanVjezba> danVjezbe = danVjezbaRepository.GetDanVjezba().Where(x => x.DanID == danID && x.RedniBrojVjezbe > redniBroj);
            foreach(var x in danVjezbe)
            {
                x.RedniBrojVjezbe--;
                danVjezbaRepository.Update(x);
            }
        }
        public void RemoveDanVjezba(DanVjezba danVjezba)
        {
            danVjezbaRepository.Remove(danVjezba);
        }
        public IEnumerable<Vjezba> getVjezbe()
        {
            return tipClanarineRepository.GetVjezbas();
        }
        public int GetMaxRedniBroj(int danID)
        {
            return danVjezbaRepository.GetDanVjezba().Where(x => x.DanID == danID).Count() + 1;
        }
        public void IncreaseRedniBroj(int danID, int redniBroj)
        {
            IEnumerable<DanVjezba> danVjezbe = danVjezbaRepository.GetDanVjezba().Where(x => x.DanID == danID && x.RedniBrojVjezbe >= redniBroj);
            foreach (var x in danVjezbe)
            {
                x.RedniBrojVjezbe++;
                danVjezbaRepository.Update(x);
            }
        }
        public void DodajDanVjezba(DanVjezba danVjezba)
        {
            danVjezbaRepository.Add(danVjezba);
        }
        public void ChangeRedniBroj(int danID, int redniBroj, int modelRedniBroj)
        {   
            if (modelRedniBroj < redniBroj)
            {
                IEnumerable<DanVjezba> danVjezbe = danVjezbaRepository.GetDanVjezba().Where(x => x.DanID == danID && x.RedniBrojVjezbe >= modelRedniBroj && x.RedniBrojVjezbe < redniBroj);
                foreach(var x in danVjezbe)
                {
                    x.RedniBrojVjezbe++;
                    danVjezbaRepository.Update(x);
                }
            }
            else if (modelRedniBroj > redniBroj)
            {
                IEnumerable<DanVjezba> danVjezbe = danVjezbaRepository.GetDanVjezba().Where(x => x.DanID == danID && x.RedniBrojVjezbe > redniBroj && x.RedniBrojVjezbe <= modelRedniBroj);
                foreach (var x in danVjezbe)
                {
                    x.RedniBrojVjezbe--;
                    danVjezbaRepository.Update(x);
                }
            }
        }
        public void UpdateDanVjezba(DanVjezba danVjezba)
        {
            danVjezbaRepository.Update(danVjezba);
        }
        public PosjecenostClana GetPosjecenostClanaByBrojKartice(string brojKartice)
        {
            return danVjezbaRepository.GetPosjecenostClana().Where(x => x.Clan.BrojKartice == brojKartice && x.VrijemeOdlaska == null).SingleOrDefault();
        }
        public Korisnik GetClanByBrojKartice(string brojKartice)
        {
            return tipClanarineRepository.GetClan().Where(x => x.BrojKartice == brojKartice).SingleOrDefault();
        }
        public void DodajPosjecenostClana(PosjecenostClana posjecenostClana)
        {
            posjecenostClanaRepository.Add(posjecenostClana);
        }
        public void UpdatePosjecenostClana(PosjecenostClana posjecenostClana)
        {
            posjecenostClanaRepository.Update(posjecenostClana);
        }
        public IEnumerable<PosjecenostClana> GetPosjecenostClana()
        {
            return posjecenostClanaRepository.GetPosjecenostClana();
        }
        public Korisnik GetClanByID(int? id)
        {
            return posjecenostClanaRepository.GetClan().Where(x => x.ID == id && x.KorisnickiNalog.Tip=="clan").SingleOrDefault();
        }
    }
}
