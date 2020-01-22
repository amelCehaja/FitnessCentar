using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazClanovaVM
    {     
        public int BrojAktivnihClanova { get; set; }
        public int UkupniBrojClanova { get; set; }    
        public List<SelectListItem> ClanarineAN { get; set; }
        public string ClanarinaAN { get; set; }
    }
}
