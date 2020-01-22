using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class Stavka
    {
        [Key]
        public int ID { get; set; }
        public string Naziv { get; set; }
        public float Cijena { get; set; }
        public string Opis { get; set; }
        public string Slika { get; set; }
        public int PodkategorijaID { get; set; }
        [ForeignKey("PodkategorijaID")]
        public Podkategorija Podkategorija { get; set; }
        public bool Obrisan { get; set; }
    }
}
