using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class TipClanarine
    {
        [Key]
        public int ID { get; set; }
        public string Naziv { get; set; }
        public float Cijena { get; set; }
        public int VrijemeTrajanja { get; set; }
        public bool Obrisan { get; set; }

    }
}
