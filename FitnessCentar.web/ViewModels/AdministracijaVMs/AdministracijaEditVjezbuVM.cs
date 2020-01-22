using FitnessCentar.web.Controllers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaEditVjezbuVM
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Opis je obavezan!")]
        [MaxLength(5000, ErrorMessage = "Maksimalna dozvoljena duzina je 5000 karaktera!")]
        public string Opis { get; set; }
        [MaxFileSize(10 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile Slika { get; set; }
    }
}
