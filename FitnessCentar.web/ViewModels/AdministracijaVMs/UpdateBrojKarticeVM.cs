using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class UpdateBrojKarticeVM
    {
        public int ID { get; set; }
        public string ImePrezime { get; set; }
        [Required(ErrorMessage = "Broj kartice je obavezan!")]
        [Remote("UniqueBrojKartice", "AdministracijaValidacija", HttpMethod = "POST", ErrorMessage = "Broj kartice postoji u bazi!")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Kartice mora sadrzavati 8 brojeva!")]
        [RegularExpression(@"^[1-9]+$", ErrorMessage = "Broj kartice dozvoljava samo brojeve!")]
        public string BrojKartice { get; set; }
    }
}
