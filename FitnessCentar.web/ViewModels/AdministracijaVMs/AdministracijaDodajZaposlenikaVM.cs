using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDodajZaposlenikaVM
    {
        [Required(ErrorMessage = "Ime je obavezno!")]
        [StringLength(20,ErrorMessage = "Makslimalna dozvoljena velicina imena je 20 karaktera!")]
        [RegularExpression(@"^[a-zA-ZŠšĐđČčĆćŽž ]+$",ErrorMessage = "Dozvoljena su samo slova!")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno!")]
        [StringLength(20, ErrorMessage = "Makslimalna dozvoljena velicina prezimena je 20 karaktera!")]
        [RegularExpression(@"^[a-zA-ZŠšĐđČčĆćŽž ]+$", ErrorMessage = "Dozvoljena su samo slova!")]
        public string Prezime { get; set; }
        [Required(ErrorMessage = "Broj telefona je obavezan!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Dozvoljeni su samo brojevi!")]
        public string BrojTelefona { get; set; }
        [Required(ErrorMessage = "Datum rodenja je obavezan!")]
        [Remote("DatumRodenjaManjiOdDanasnjeg","AdministracijaValidacija",HttpMethod = "POST",ErrorMessage = "Datum rođenja mora biti manji od današnjeg!")]
        public DateTime DatumRodenja { get; set; }
        [Remote("JMBGValidate", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "JMBG moze sadrzavati samo brojeve i mora biti duzine od 14 brojeva!")]
        public string JMBG { get; set; }
        [Required(ErrorMessage = "Spol je obavezan!")]
        [StringLength(1, MinimumLength = 1)]
        [RegularExpression(@"[MŽ]")]
        public string Spol { get; set; }
        public List<SelectListItem> spol { get; set; }
        [Required(ErrorMessage = "Email je obavezan!")]
        [Remote("Email", "AdministracijaValidacija")]
        public string Email { get; set; }
    }
}
