using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class MjerenjeNapredka
    {
        [Key]
        public int ID { get; set; }
        public DateTime DatumMjerenja { get; set; }
        public string Kilaza { get; set; }
        public string Visina { get; set; }
        public string ProcenatMasti { get; set; }
        public string ObimPrsa { get; set; }
        public string ObimRuku { get; set; }
        public string ObimStruka { get; set; }
        public string ObimNoge { get; set; }
        public int ClanID { get; set; }
        [ForeignKey("ClanID")]
        public Korisnik Clan { get; set; }
    }
}
