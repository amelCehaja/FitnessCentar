using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaUrediStavkuVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Naziv je obavezan!")]
        [MaxLength(50, ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [Remote("UniquePodkategorija", "AdministracijaValidacija", HttpMethod = "post", ErrorMessage = "Naziv postoji u bazi podataka!")]
        [RegularExpression(@"^[a-zA-Z0-9ČčĆćŽžŠšĐđ ]+$", ErrorMessage = "Dozvoljena su samo slova i brojevi!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Cijena je obavezna!")]
        [Range(0, float.MaxValue, ErrorMessage = "Cijena ne moze biti negativna!")]
        public float Cijena { get; set; }
        [MaxLength(5000, ErrorMessage = "Maksimalna dozvoljena duzina je 5000 karaktera!")]
        public string Opis { get; set; }
        public string Kategorija { get; set; }
        public string Podkategorija { get; set; }
    }
}
