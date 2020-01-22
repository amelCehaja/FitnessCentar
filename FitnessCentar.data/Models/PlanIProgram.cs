using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class PlanIProgram
    {
        [Key]
        public int ID { get; set; }
        public string Naziv { get; set; }
        public int KategorijaID { get; set; }
        [ForeignKey("KategorijaID")]
        public KategorijaPlanIProgram Kategorija { get; set; }
        public string Opis { get; set; }
        public bool Obrisan { get; set; }
    }
}
