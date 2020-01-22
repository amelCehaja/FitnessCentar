using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDodajPlanIProgramVM
    {
        [Required(ErrorMessage ="Naziv je obavezan!")]
        [MaxLength(50,ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [RegularExpression(@"^[a-zA-Z0-9ČčĆćŽžŠšĐđ ]+$",ErrorMessage ="Dozvoljena su samo slova i brojevi!")]
        [Remote("UniquePlanIProgram", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "Naziv postoji u bazi podataka!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Opis je obavezan!")]
        [MaxLength(5000, ErrorMessage = "Maksimalna dozvoljena duzina je 5000 karaktera!")]
        public string Opis { get; set; }
        public List<SelectListItem> Kategorije { get; set; }
        [Required]
        public int KategorijaId { get; set; }
        [Required(ErrorMessage ="Broj sedmica je obavezan!")]
        [Range(1,15, ErrorMessage ="Mora biti u rasponu od 1 do 15!")]
        public int BrojSedmica { get; set; }
       
    }
}
