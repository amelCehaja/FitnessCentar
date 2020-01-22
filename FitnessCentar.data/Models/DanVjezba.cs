using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class DanVjezba
    {
        [Key]
        public int ID { get; set; }
        public int DanID { get; set; }
        [ForeignKey("DanID")]
        public Dan Dan { get; set; }
        public int VjezbaID { get; set; }
        [ForeignKey("VjezbaID")]
        public Vjezba Vjezba { get; set; }
        public int BrojPonavljanja { get; set; }
        public int BrojSetova { get; set; }
        public int? DuzinaOdmora { get; set; }
        public int RedniBrojVjezbe { get; set; }
    }
}
