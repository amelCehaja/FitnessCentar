using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazKategorijaVM
    {
        public class Kategorija
        {
            public int ID { get; set; }
            public string Naziv { get; set; }
            public List<string> Podkategorije { get; set; }
        }
        public List<Kategorija> Kategorije { get; set; }
    }
}
