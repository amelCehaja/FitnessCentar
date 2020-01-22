using FitnessCentar.web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDodajVjezbuVM
    {
        [Required(ErrorMessage = "Naziv je obavezan!")]
        [MaxLength(50,ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [RegularExpression(@"^[a-zA-Z0-9ČčĆćŽžĐđŠš ]+$", ErrorMessage = "Dozvoljena su samo slova i brojevi!")]
        [Remote("UniqueVjezba", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "Naziv postoji u bazi podataka!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Opis je obavezan!")]
        [MaxLength(5000,ErrorMessage = "Maksimalna dozvoljena duzina je 5000 karaktera!")]
        public string Opis { get; set; }
        [Required(ErrorMessage = "Slika je obavezna!")]
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg"})]
        public IFormFile Slika { get; set; }
    }
}
