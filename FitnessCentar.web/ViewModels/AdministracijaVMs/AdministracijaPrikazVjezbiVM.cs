using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazVjezbiVM
    {
        public class Vjezba
        {
            public int ID { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public string Slika { get; set; }
        }

        public List<Vjezba> Vjezbe { get; set; }
    }
}
