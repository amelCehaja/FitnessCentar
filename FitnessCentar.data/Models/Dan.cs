using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCentar.data.Models
{
    public class Dan
    {
        [Key]
        public int ID { get; set; }
        public int SedmicaID { get; set; }
        [ForeignKey("SedmicaID")]
        public Sedmica Sedmica { get; set; }
        public int RedniBroj { get; set; }
        public int DuzinaTreninga { get; set; }
    }
}
