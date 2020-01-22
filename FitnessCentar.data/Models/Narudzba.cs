using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class Narudzba
    {
        [Key]
        public int ID { get; set; }
        public DateTime DatumVrijeme { get; set; }
        public int? RacunID { get; set; }
        [ForeignKey("RacunID")]
        public Racun Racun { get; set; }
        public int NacinPlacanjaID { get; set; }
        [ForeignKey("NacinPlacanjaID")]
        public NacinPlacanja NacinPlacanja { get; set; }
        public int Cijena { get; set; }
        public int KorisnikID { get; set; }
        [ForeignKey("KorisnikID")]
        public Korisnik Korisnik { get; set; }
        public int AdresaID { get; set; }
        [ForeignKey("AdresaID")]
        public Adresa Adresa { get; set; }
    }
}
