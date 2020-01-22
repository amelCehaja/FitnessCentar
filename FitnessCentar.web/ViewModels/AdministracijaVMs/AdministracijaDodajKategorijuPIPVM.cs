using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDodajKategorijuPIPVM
    {
        public int? ID { get; set; }
        [Required(ErrorMessage = "Naziv je obavezan!")]
        [Remote("UniqueKategorijaPIP", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "Naziv postoji u bazi podataka!")]
        [MaxLength(50,ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [RegularExpression(@"^[A-Za-z0-9ĐđŠšŽžŠČčĆć ]+$",ErrorMessage = "Dozvoljena su samo slova i brojevi!")]
        public string Naziv { get; set; }
    }
}
