using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazRacunaVM
    {
        public List<Row> Racuni { get; set; }
        public class Row
        {
            public int ID { get; set; }
            public string VrijemeNarudzbe { get; set; }
            public string DatumIsporuke { get; set; }
            public string BrojStavki { get; set; }
            public string Cijena { get; set; }
        }
    }
}
