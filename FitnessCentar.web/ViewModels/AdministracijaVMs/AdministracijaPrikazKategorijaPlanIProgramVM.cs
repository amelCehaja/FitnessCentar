using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazKategorijaPlanIProgramVM
    {
        public class Row
        {
            public int ID { get; set; }
            public string Naziv { get; set; }
        }
        public List<Row> kategorije { get; set; }
    }
}
