using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class AutorizacijskiToken
    {
        [Key]
        public int ID { get; set; }
        public string Vrijednost { get; set; }
        public int KorisnickiNalogID { get; set; }
        [ForeignKey("KorisnickiNalogID")]
        public KorisnickiNalog KorisnickiNalog { get; set; }
        public DateTime VrijemeEvidentiranja { get; set; }
    }
}
