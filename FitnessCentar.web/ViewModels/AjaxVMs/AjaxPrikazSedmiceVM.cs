using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxPrikazSedmiceVM
    {
        public int? SedmicaID { get; set; }
        public class Row
        {
            public int ID { get; set; }
            public int RedniBrojDana { get; set; }
            public int DuzinaTreninga { get; set; }
        }
        public List<Row> Dani { get; set; }
    }
}
