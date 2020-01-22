using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class Clanarina
    {
        [Key]
        public int ID { get; set; }
        public DateTime DatumDodavanja { get; set; }
        public DateTime DatumIsteka { get; set; }      
        public int TipClanarineID { get; set; }
        [ForeignKey("TipClanarineID")]
        public TipClanarine TipClanarine { get; set; }       
        public int? ClanID { get; set; }
        [ForeignKey("ClanID")]
        public Korisnik Clan { get; set; }
    }
}
