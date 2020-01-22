using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class Adresa
    {
        [Key]
        public int ID { get; set; }
        public string Grad { get; set; }
        public string Ulica { get; set; }
        public string PostanskiBroj { get; set; }
        public int KorisnikID { get; set; }
        [ForeignKey("KorisnikID")]
        public Korisnik Korisnik { get; set; }

    }
}
