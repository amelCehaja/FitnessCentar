using FitnessCentar.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessCentar.service.Interfaces
{
    public interface IZaposlenikService
    {
        void DodajZaposlenika(Korisnik korisnik);
        Korisnik ZaposlenikFind(int id);
        void ObrisiZaposlenik(Korisnik korisnik);
        IEnumerable<Korisnik> GetKorisnike();
    }
}
