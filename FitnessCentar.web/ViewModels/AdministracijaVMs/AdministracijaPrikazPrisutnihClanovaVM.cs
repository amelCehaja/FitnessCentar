using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazPrisutnihClanovaVM
    {
        public class Row
        {
            public int ID { get; set; }
            public string Ime { get; set; }
            public string VrijemeDolaska { get; set; }
            public string Slika { get; set; }
            public string AktivnaClanarina { get; set; }
        }
        public List<Row> Clanovi { get; set; }     
    }
}
