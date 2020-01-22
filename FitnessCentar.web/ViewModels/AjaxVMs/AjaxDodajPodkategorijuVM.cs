using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxDodajPodkategorijuVM
    {
        [Required(ErrorMessage = "Naziv je obavezan!")]
        [MaxLength(50, ErrorMessage = "Maksimalna dozvoljena duzina je 50 karaktera!")]
        [Remote("UniquePodkategorija", "AdministracijaValidacija", HttpMethod = "post", ErrorMessage = "Naziv postoji u bazi podataka!")]
        [RegularExpression(@"^[a-zA-Z0-9ČčĆćŽžŠšĐđ ]+$", ErrorMessage = "Dozvoljena su samo slova i brojevi!")]
        public string Naziv { get; set; }
        public string Kategorija { get; set; }
        public int KategorijaID { get; set; }
    }
}
