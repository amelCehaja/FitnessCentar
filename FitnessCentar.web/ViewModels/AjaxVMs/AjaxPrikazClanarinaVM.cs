using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxPrikazClanarinaVM
    {
        public class Clanarina
        {
            public string ImePrezime { get; set; }
            public string TipClanarine { get; set; }
            public string DatumDodavanja { get; set; }
            public string DatumIsteka { get; set; }
        }
        public List<Clanarina> clanovi { get; set; }        
    }
}
