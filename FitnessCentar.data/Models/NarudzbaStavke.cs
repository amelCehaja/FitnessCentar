using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class NarudzbaStavke
    {
        [Key]
        public int ID { get; set; }
        public int NarudzbaID { get; set; }
        [ForeignKey("NarudzbaID")]
        public Narudzba Narudzba { get; set; }
        public int StavkaID { get; set; }
        [ForeignKey("StavkaID")]
        public Stavka Stavka { get; set; }
        public int Kolicina { get; set; }
        public float Cijena { get; set; }
        public int? VelicinaID { get; set; }
        [ForeignKey("VelicinaID")]
        public Skladiste Velicina { get; set; }
    }
}
