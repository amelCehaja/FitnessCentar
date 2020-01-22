using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCentar.web.ViewModels.Autentifikacija
{
    public class RegisterVM
    {
        [Required]
        [StringLength(100, ErrorMessage = "Ime mora sadržavati mininalno 3 karaktera.", MinimumLength = 3)]
        public string Ime { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Prezime mora sadržavati mininalno 3 karaktera.", MinimumLength = 3)]
        public string Prezime { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Email mora sadržavati mininalno 3 karaktera.", MinimumLength = 3)]
        [Remote("CheckEmail", "KlijentValidacija")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Korisničko ime mora sadržavati mininalno 3 karaktera.", MinimumLength = 3)]
        [Remote("CheckUsername", "KlijentValidacija")]
        public string KorisnickoIme { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Password mora sadržavati mininalno 4 karaktera.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Password mora sadržavati mininalno 4 karaktera.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Compare(nameof(Lozinka))]
        public string LozinkaPonovi { get; set; }
        public string Spol { get; set; }
        public List<SelectListItem> SpolList { get; set; }
        public DateTime DatumRodjenja { get; set; }
    }
}
