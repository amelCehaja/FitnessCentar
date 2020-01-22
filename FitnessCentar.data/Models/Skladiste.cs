using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class Skladiste
    {
        [Key]
        public int ID { get; set; }
        public int Kolicina { get; set; }
        public string Velicina { get; set; }
        public int StavkaID { get; set; }
        [ForeignKey("StavkaID")]
        public Stavka Stavka { get; set; }
    }
}
