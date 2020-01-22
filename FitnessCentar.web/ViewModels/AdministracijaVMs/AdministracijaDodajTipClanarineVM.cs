using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDodajTipClanarineVM
    {
        [Required(ErrorMessage = "Naziv je obavezan!")]
        [Remote("UniqueNazivTipClanarine","AdministracijaValidacija",HttpMethod = "POST",ErrorMessage ="Naziv već postoji !!!")]
        [RegularExpression(@"^[a-zA-Z0-9ČčĆćŽžŠšĐđ ]+$", ErrorMessage = "Dozvoljena su samo slova i brojevi!")]
        [MaxLength(50, ErrorMessage ="Maksimalna dozvoljena duzina je 50 karaktera!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Cijena je obavezna!")]
        [Range(0.01,float.MaxValue,ErrorMessage ="Cijena ne moze biti 0 ili u minusu!")]
        public float Cijena { get; set; }
        [Required(ErrorMessage = "Trajanje je obavezno!")]
        [Range(1,365,ErrorMessage = "Trajanje mora biti u rasponu od 1 do 365!")]
        public int Trajanje { get; set; }
    }
}
