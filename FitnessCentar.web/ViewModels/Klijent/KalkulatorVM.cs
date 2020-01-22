using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCentar.web.ViewModels.Klijent
{
    public class KalkulatorVM
    {
        [Required]
        public string Spol { get; set; }
        public List<SelectListItem> SpolList { get; set; }
        public int Starost { get; set; }
        public int Tezina { get; set; }
        public int Visina { get; set; }
        [Required]
        public string Aktivnost { get; set; }
        public List<SelectListItem> AktivnostList { get; set; }
        public int? UdioMasnoce { get; set; }
    }
}
