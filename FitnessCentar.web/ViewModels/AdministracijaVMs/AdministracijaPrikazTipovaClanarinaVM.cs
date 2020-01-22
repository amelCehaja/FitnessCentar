using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazTipovaClanarinaVM
    {
        public class TipClanarine
        {
            public int ID { get; set; }
            public string Naziv { get; set; }
            public string Cijena { get; set; }
            public string VrijemeTrajanja { get; set; }
        }
        public List<TipClanarine> tipoviClanarine { get; set; }
    }
}
