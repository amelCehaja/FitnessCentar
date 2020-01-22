using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCentar.web.ViewModels.Klijent
{
    public class KalkulatorRezultatVM
    {
        public double Tdee { get; set; }
        public double Bmi { get; set; }
        public string BmiKategorija { get; set; }
        public List<SelectListItem> BmiKategorijaList { get; set; }
        public int Starost { get; set; }
        public int Tezina { get; set; }
        public int Visina { get; set; }
        public int? UdioMasnoce { get; set; }
    }
}
