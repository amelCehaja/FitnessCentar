using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxPrikazDanaVM
    {
        public int? DanID { get; set; }
        public List<Vjezba> Vjezbe { get; set; }
        public class Vjezba
        {
            public int ID { get; set; }
            public string Naziv { get; set; }
            public int BrojPonavljanja { get; set; }
            public int BrojSetova { get; set; }
            public int RedniBrojVjezbe { get; set; }
        }
    }
}
