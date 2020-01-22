using FitnessCentar.data;
using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitnessCentar.service.Services
{
    public class ZaposlenikService : IZaposlenikService
    {
        IRepository<Korisnik> korisnikRepository;
        public ZaposlenikService(IRepository<Korisnik> _korisnikRepository)
        {
            korisnikRepository = _korisnikRepository;
        }
        public void DodajZaposlenika(Korisnik korisnik)
        {
            korisnikRepository.Add(korisnik);
        }
        public Korisnik ZaposlenikFind(int id)
        {
            return korisnikRepository.GetZaposlenik().Where(x => x.ID == id && x.KorisnickiNalog.Tip == "zaposlenik").SingleOrDefault();
        }
        public void ObrisiZaposlenik(Korisnik korisnik)
        {
            korisnikRepository.Remove(korisnik);
        }
        public IEnumerable<Korisnik> GetKorisnike()
        {
            return korisnikRepository.GetZaposlenik();
        }
    }
}
