using FitnessCentar.web.Controllers;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxDodajSkladisteVM
    {
        public int StavkaID { get; set; }
        public int? SkladisteID { get; set; }
        [Required(ErrorMessage = "Velicina je obavezna!")]
        public string Velicina { get; set; }
        [Required(ErrorMessage = "Kolicina je obavezna!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Dozvoljeni su samo brojevi!")]
        [Range(0,double.MaxValue,ErrorMessage = "Kolicina ne smije biti negativna!")]
        public int Kolicina { get; set; }
    }
}
