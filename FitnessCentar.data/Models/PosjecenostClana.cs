using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitnessCentar.data.Models
{
    public class PosjecenostClana
    {
        [Key]
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public DateTime VrijemeDolaska { get; set; }
        public DateTime? VrijemeOdlaska { get; set; }
        public int ClanID { get; set; }
        [ForeignKey("ClanID")]
        public Korisnik Clan { get; set; }
    }
}
