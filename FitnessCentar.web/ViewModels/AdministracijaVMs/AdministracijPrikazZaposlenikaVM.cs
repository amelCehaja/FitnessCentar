using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijPrikazZaposlenikaVM
    {
        public List<Zaposlenik> zaposlenici { get; set; }
        public class Zaposlenik
        {
            public int ID { get; set; }
            public string ImePrezime { get; set; }
            public string DatumRodenja { get; set; }
        }
    }
}
