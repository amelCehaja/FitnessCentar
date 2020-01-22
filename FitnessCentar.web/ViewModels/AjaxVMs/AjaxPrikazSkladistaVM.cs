using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxPrikazSkladistaVM
    {
        public class Row
        {
            public int ID { get; set; }
            public int Kolicina { get; set; }
            public string Velicina { get; set; }
        }
        public List<Row> skladiste { get; set; }
        public int StavkaID { get; set; }
    }
}
