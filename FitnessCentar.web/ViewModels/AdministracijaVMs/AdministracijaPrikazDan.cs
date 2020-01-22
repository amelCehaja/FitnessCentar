using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazDanVM
    {
        public int SedmicaID { get; set; }
        public int DanID { get; set; }
        public int PlanID { get; set; }
        public List<SelectListItem> Sedmice { get; set; }
        public List<SelectListItem> Dani { get; set; }
        public class VjezbaDan
        {
            public int ID { get; set; }
            public int BrojPonavaljanja { get; set; }
            public int BrojSetova { get; set; }
            public int DuzinaOdmora { get; set; }
            public int RedniBrojVjezbe { get; set; }
        }
    }
}
