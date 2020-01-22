using FitnessCentar.web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDodajStavkuVM
    {
        [Required(ErrorMessage = "Naziv je obavezan!")]
        [MaxLength(50, ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [Remote("UniqueStavka", "AdministracijaValidacija", HttpMethod = "post", ErrorMessage = "Naziv postoji u bazi podataka!")]
        [RegularExpression(@"^[a-zA-Z0-9ČčĆćŽžŠšĐđ ]+$", ErrorMessage = "Dozvoljena su samo slova i brojevi!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Cijena je obavezna!")]
        [Range(0.1,float.MaxValue,ErrorMessage ="Cijena mora biti veca od 0!")]
        public float Cijena { get; set; }
        [MaxLength(5000, ErrorMessage = "Maksimalna dozvoljena duzina je 5000 karaktera!")]
        public string Opis { get; set; }
        public List<SelectListItem> Kategorije { get; set; }
        [Required(ErrorMessage = "Slika je obavezna!")]
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile Slika { get; set; }
        [Required(ErrorMessage = "Podkategorija je obavezna!")]
        public int PodkategorijaID { get; set; }
    }
}
