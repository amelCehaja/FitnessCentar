using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxUrediPiPVM
    {
        public int ID { get; set; }     
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Opis je obavezan!")]
        [StringLength(5000,ErrorMessage = "Maksimalna dozvoljena duzina je 5000 karaktera!")]
        public string Opis { get; set; }
        public string Kategorija { get; set; }
    }
}
