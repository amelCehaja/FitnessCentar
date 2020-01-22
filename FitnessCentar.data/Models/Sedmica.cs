using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FitnessCentar.data.Models
{
    public class Sedmica
    {
        [Key]
        public int ID { get; set; }
        public int PlanIProgramID { get; set; }
        [ForeignKey("PlanIProgramID")]
        public PlanIProgram PlanIProgram { get; set; }
        public int RedniBroj { get; set; }
    }
}
