using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class Racun
    {
        [Key]
        public int ID { get; set; }
        public DateTime DatumVrijeme { get; set; }
        public int ZaposlenikID { get; set; }
        [ForeignKey("ZaposlenikID")]
        public Korisnik Zaposlenik { get; set; }
    }
}
