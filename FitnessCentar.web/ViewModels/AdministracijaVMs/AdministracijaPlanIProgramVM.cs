using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPlanIProgramVM
    {
        public List<Row> Planovi { get; set; }
        public class Row
        {
            public int ID { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public string Kategorija { get; set; }
        }
    }
}
