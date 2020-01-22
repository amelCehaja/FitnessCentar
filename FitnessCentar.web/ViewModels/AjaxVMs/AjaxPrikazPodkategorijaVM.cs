using System.Collections.Generic;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxPrikazPodkategorijaVM
    {
        public class Row
        {
            public int ID { get; set; }
            public string Naziv { get; set; }
        }
        public List<Row> Podkategorije { get; set; }
        public int KategorijaID { get; set; }
    }
}
