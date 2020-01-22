using FitnessCentar.data.Models;
using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDetaljiClanaVM
    {
        public Korisnik clan { get; set; }
        public Clanarina clanarina { get; set; }
        public string DatumPrveClanarine { get; set; }
        public List<Row> Clanarine { get; set; }
        public class Row
        {
            public string DatumDodavanja { get; set; }
            public string DatumIsteka { get; set; }
            public string TipClanarine { get; set; }
        }
    }
}
