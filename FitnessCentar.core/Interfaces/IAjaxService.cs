using FitnessCentar.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessCentar.service.Interfaces
{
    public interface IAjaxService
    {
        int TrajanjeTipaClanarine(int id);
        List<Clanarina> GetClanarineByStatus(string status);
        IEnumerable<Korisnik> PrikazClanovaPoImenu(string imePrezime);
        IEnumerable<Podkategorija> GetPodkategorijeByKategorijaID(int kategorijaID);
        int GetIDOfFirstWeekInPlan(int? planID);
        IEnumerable<Dan> GetDaniBySedmicaID(int? sedmicaID);
        int GetIDOfFirstDayInWeek(int? sedmicaID);
        IEnumerable<DanVjezba> GetDanVjezbeOrderdByRedniBroj(int? danID);
        DanVjezba DanVjezbaFind(int id);
        void UpdateRedniBroj(int danID, int redniBroj);
        void RemoveDanVjezba(DanVjezba danVjezba);
        IEnumerable<Vjezba> getVjezbe();
        int GetMaxRedniBroj(int danID);
        void IncreaseRedniBroj(int danID, int redniBroj);
        void DodajDanVjezba(DanVjezba danVjezba);
        void ChangeRedniBroj(int danID, int redniBroj, int modelRedniBroj);
        void UpdateDanVjezba(DanVjezba danVjezba);
        PosjecenostClana GetPosjecenostClanaByBrojKartice(string brojKartice);
        Korisnik GetClanByBrojKartice(string brojKartice);
        void DodajPosjecenostClana(PosjecenostClana posjecenostClana);
        void UpdatePosjecenostClana(PosjecenostClana posjecenostClana);
        IEnumerable<PosjecenostClana> GetPosjecenostClana();
        Korisnik GetClanByID(int? id);
    }
}
