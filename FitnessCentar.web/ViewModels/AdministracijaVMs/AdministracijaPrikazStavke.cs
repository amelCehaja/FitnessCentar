using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazStavke
    {
        public List<Row> Stavke { get; set; }
        public class Row
        {
            public int ID { get; set; }
            public string Naziv { get; set; }
            public string Cijena { get; set; }
            public string Podkategorija { get; set; }
            public string Kategorija { get; set; }
            public string Slika { get; set; }
            public List<string> Skladiste { get; set; }
        }     
    }
}
