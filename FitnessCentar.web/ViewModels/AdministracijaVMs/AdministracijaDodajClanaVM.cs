using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDodajClanaVM
    {
        [Required(ErrorMessage = "Ime je obavezno!")]
        [StringLength(50, ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [RegularExpression(@"^[a-zA-ZČčĆćŽžĐđŠš ]+$", ErrorMessage = "Dozvoljena su samo slova!")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno!")]
        [StringLength(50, ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [RegularExpression(@"^[a-zA-ZČčĆćŽžĐđŠš ]+$", ErrorMessage = "Dozvoljena su samo slova!")]
        public string Prezime { get; set; }
        [Required(ErrorMessage = "Email je obavezan!")]
        [Remote("Email", "AdministracijaValidacija")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Broj telefona je obavezan!")]
        [StringLength(15, ErrorMessage = "Maksimalna dozvoljena duzina broja telefona je 15 brojeva!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "DOzvoljeni su samo brojevi!")]
        public string BrojTelefona { get; set; }
        [Required(ErrorMessage = "Datum rodenja je obavezan!")]
        [Remote("DatumRodenjaManjiOdDanasnjeg", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "Datum rođenja mora biti manji od današnjeg!!!")]
        public DateTime DatumRodenja { get; set; }
        public string Slika { get; set; }
        [Required(ErrorMessage = "Broj kartice je obavezan!")]
        [Remote("UniqueBrojKartice","AdministracijaValidacija",HttpMethod = "POST",ErrorMessage = "Broj kartice postoji u bazi!")]
        [StringLength(8,MinimumLength = 8,ErrorMessage = "Broj kartice mora sadrzavati 8 brojeva!")]
        [RegularExpression(@"^[0-9]+$",ErrorMessage = "Dozvoljeni su samo brojevi!")]
        public string BrojKartice { get; set; }
        [Required(ErrorMessage = "Spol je obavezan!")]
        [StringLength(1, MinimumLength = 1)]
        [RegularExpression(@"[MŽ]")]
        public string Spol { get; set; }
        [Remote("JMBGValidate", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "JMBG moze sadrzavati samo slova i mora biti duzine od 14 brojeva!")]
        public string JMBG { get; set; }
        public List<SelectListItem> spol { get; set; }
        [Required(ErrorMessage = "Datum dodavanja je obavezan!")]
        [Remote("DatumDodavanjaValidation", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "Datum dodavanja ne smije biti manji od danasnjeg dana!")]
        public DateTime DatumDodavanja { get; set; }
        public DateTime DatumIsteka { get; set; }
        public int TipClanarineID { get; set; }
        public List<SelectListItem> clanarine { get; set; }
    }

}
