using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDetaljiNarudzbaVM
    {
        public class Row
        {
            public string Naziv { get; set; }
            public string Kategorija { get; set; }
            public string Kolicina { get; set; }
            public string UkupnaCijena { get; set; }
            public string Slika { get; set; }
        }
        public List<Row> Stavke { get; set; }
        public string Adresa { get; set; }
        public string Kupac { get; set; }
        public bool Isporuceno { get; set; }
        public int ID { get; set; }
    }
}
