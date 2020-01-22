using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class Korisnik
    {
        [Key]
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public DateTime DatumRodenja { get; set; }
        public string JMBG { get; set; }
        public string Slika { get; set; }
        public string BrojKartice { get; set; }
        public string Spol { get; set; }
        public string Email { get; set; }
        public int? KorisnickiNalogID { get; set; }
        [ForeignKey("KorisnickiNalogID")]
        public KorisnickiNalog KorisnickiNalog { get; set; }
        public bool Obrisan { get; set; }
    }

}
