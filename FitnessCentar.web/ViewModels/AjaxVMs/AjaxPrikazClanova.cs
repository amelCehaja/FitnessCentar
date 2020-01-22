using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxPrikazClanova
    {
        public class Clan
        {
            public int id { get; set; }
            public string ImePrezime { get; set; }
            public string Email { get; set; }
            public string datumIstekaClanarine { get; set; }
            public string Slika { get; set; }
            public string TipClanarine { get; set; }
        }
        public List<Clan> clanovi { get; set; }
    }
}
