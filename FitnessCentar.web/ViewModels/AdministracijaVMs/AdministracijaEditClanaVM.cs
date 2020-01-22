using FitnessCentar.web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaEditClanaVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Ime je obavezno!")]
        [StringLength(50,ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [RegularExpression(@"^[a-zA-ZČčĆćŽžĐđŠš ]+$",ErrorMessage = "Dozvoljena su samo slova!")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno!")]
        [StringLength(50,ErrorMessage ="Maksimalna dozvoljena duzina je 50 karaktera!")]
        [RegularExpression(@"^[a-zA-ZČčĆćŽžĐđŠš ]+$", ErrorMessage = "Dozvoljena su samo slova!")]
        public string Prezime { get; set; }
        [Required(ErrorMessage = "Broj telefona je obavezan!")]
        [StringLength(15,ErrorMessage = "Maksimalna dozvoljena duzina broja telefona je 15 brojeva!")]
        [RegularExpression(@"^[0-9]+$",ErrorMessage ="DOzvoljeni su samo brojevi!")]
        public string BrojTelefona { get; set; }
        [Required(ErrorMessage = "Datum rodenja je obavezan!")]
        [Remote("DatumRodenjaManjiOdDanasnjeg","AdministracijaValidacija",HttpMethod = "POST",ErrorMessage = "Datum rođenja mora biti manji od današnjeg!!!")]
        public DateTime DatumRodenja { get; set; }
        public string BrojKartice { get; set; }
        [Required(ErrorMessage = "Spol je obavezan!")]
        [StringLength(1, MinimumLength = 1)]
        [RegularExpression(@"[MŽ]")]
        public string Spol { get; set; }
        [Remote("JMBGValidate", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "JMBG moze sadrzavati samo brojeve i mora biti duzine od 14 brojeva!")]
        public string JMBG { get; set; }
        public List<SelectListItem> spol { get; set; }
    }
    
}
