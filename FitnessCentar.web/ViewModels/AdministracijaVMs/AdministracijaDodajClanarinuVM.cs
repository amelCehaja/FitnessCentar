using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessCentar.web.ViewModels.AdministracijaVMs
{
    public class AdministracijaDodajClanarinuVM
    {
        public int ClanID { get; set; }
        public string ImePrezime { get; set; }
        public List<SelectListItem> TipoviClanarine { get; set; }
        public int TipClanarineID { get; set; }
        [Required(ErrorMessage = "Datum dodavanja je obavezan!")]
        [Remote("DatumDodavanjaValidation","AdministracijaValidacija",HttpMethod = "POST",ErrorMessage = "Datum dodavanja ne smije biti manji od danasnjeg dana!")]
        public DateTime DatumDodavanja { get; set; }
        public DateTime DatumIsteka { get; set; }
    }
}
