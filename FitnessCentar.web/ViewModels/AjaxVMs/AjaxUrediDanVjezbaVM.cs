using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AjaxVMs
{
    public class AjaxUrediDanVjezbaVM
    {
        public int ID { get; set; }
        public int DanID { get; set; }
        public string Vjezba { get; set; }
        [Required(ErrorMessage = "Broj setova je obavezan")]
        [Range(1, 50, ErrorMessage = "Broj setova mora biti u rasponu od 1 do 50!")]
        public int BrojSetova { get; set; }
        [Required(ErrorMessage = "Broj ponavljanja je obavezan!")]
        [Range(1, 150, ErrorMessage = "Broj ponavljanja mora biti u rasponu od 1 do 150!")]
        public int BrojPonavljanja { get; set; }
        [Range(1, 500, ErrorMessage = "Duzina odmora mora biti u razmaku od 1 do 500 sekundi!")]
        public int? DuzinaOdmora { get; set; }
        [Required(ErrorMessage = "Redni broj je obavezan!")]
        public int RedniBroj { get; set; }
        public List<SelectListItem> RedniBrojevi { get; set; }
    }
}
