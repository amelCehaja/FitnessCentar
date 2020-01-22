using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaPrikazPlanIProgramVM
    {
        public int ID { get; set; }
        public List<Sedmica> Sedmice { get; set; }
        public string Naziv { get; set; }
        public string Kategorija { get; set; }
        public string Opis { get; set; }
        public class Sedmica
        {
            public int ID { get; set; }
            public int RedniBroj { get; set; }
            public List<Dan> Dani { get; set; }
        }
        public class Dan
        {
            public int ID { get; set; }
            public int RedniBroj { get; set; }
        }
    }
   
}
