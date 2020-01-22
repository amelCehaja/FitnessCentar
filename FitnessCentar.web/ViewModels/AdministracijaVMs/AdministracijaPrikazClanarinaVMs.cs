using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazClanarinaVMs
    {
        public List<SelectListItem> tipoviClanarine { get; set; }
        public List<SelectListItem> statusi { get; set; }
        public int UkupnoAktivnih { get; set; }
    }
}
