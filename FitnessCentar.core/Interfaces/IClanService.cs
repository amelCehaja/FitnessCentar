using FitnessCentar.data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;


namespace FitnessCentar.service.Interfaces
{
    public interface IClanService
    {
        List<SelectListItem> GetTipoviClanarineSelectList();
        void DodajClana(Korisnik clan);
        void DodajClanarinu(Clanarina clanarina);
        Korisnik ClanFind(int id);
        int BrojAktivnihClanarina();
        int UkupanBrojClanova();
        List<SelectListItem> GetTipoviClanarine();
        void ClanUpdate(Korisnik clan);
        IEnumerable<TipClanarine> GetAllTipoviClanarine();
        TipClanarine TipClanarineFind(int id);
        void DodajTipClanarine(TipClanarine tipClanarine);
        void UpdateTipClanarine(TipClanarine tipClanarine);
        Clanarina AktivnaClanarina(int clanID);
        string DatumPrveClanarine(int clanID);
        IEnumerable<Clanarina> GetClanarineByClanID(int id);
        bool IsUsernameUnique(string username);
        void DodajKorisnickiNalog(KorisnickiNalog korisnickiNalog);
    }
}
