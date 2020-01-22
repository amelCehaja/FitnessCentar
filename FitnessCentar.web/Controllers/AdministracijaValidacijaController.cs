using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using FitnessCentar.data.EF;
using FitnessCentar.data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCentar.web.Controllers
{
    public class AdministracijaValidacijaController : Controller
    {
        private MyContext db;
        public AdministracijaValidacijaController(MyContext myContext)
        {
            db = myContext;
        }
        public bool DatumDodavanjaValidation(DateTime DatumDodavanja)
        {
            DateTime danas = DateTime.Now.Date;
            if (DatumDodavanja < danas)
            {
                return false;
            }
            return true;
        }
        public bool UniqueNazivTipClanarine(string Naziv)
        {
            TipClanarine tipClanarine = db.TipClanarine.Where(t => t.Naziv == Naziv && t.Obrisan == false).FirstOrDefault();
            if (tipClanarine == null)
            {
                return true;
            }
            return false;
        }
        public bool UniqueKategorija(string Naziv)
        {
            Kategorija kategorija = db.Kategorija.Where(x => x.Naziv == Naziv && x.Obrisan == false).FirstOrDefault();
            if (kategorija == null)
            {
                return true;
            }
            return false;
        }
        public bool DatumRodenjaManjiOdDanasnjeg([Bind(Prefix = "clan.DatumRodenja")]DateTime DatumRodenja)
        {
            DateTime danas = DateTime.Now;
            if (DatumRodenja < danas)
            {
                return true;
            }
            return false;
        }
        public bool UniquePlanIProgram(string Naziv)
        {
            PlanIProgram planIProgram = db.PlanIProgram.Where(x => x.Naziv == Naziv && x.Obrisan==false).FirstOrDefault();
            if (planIProgram == null)
            {
                return true;
            }
            return false;
        }
        public bool PostojanjeKategorijePlanIProgram(int KategorijaId)
        {
            KategorijaPlanIProgram kategorija = db.KategorijaPlanIProgram.Find(KategorijaId);
            if (kategorija == null)
            {
                return false;
            }
            return true;
        }
        public bool UniquePodkategorija(string Naziv)
        {
            Podkategorija podkategorija = db.Podkategorija.Where(x => x.Naziv == Naziv && x.Obrisan == false).FirstOrDefault();
            if (podkategorija == null)
            {
                return true;
            }
            return false;
        }
        public bool PostojanjeKategorija(int KategorijaID)
        {
            Kategorija kategorija = db.Kategorija.Find(KategorijaID);
            if (kategorija == null)
            {
                return false;
            }
            return true;
        }
        public bool UniqueStavka(string Naziv)
        {
            Stavka stavka = db.Stavka.Where(x => x.Naziv == Naziv && x.Obrisan == false).FirstOrDefault();
            if (stavka == null)
            {
                return true;
            }
            return false;
        }
        public bool PostojanjePodkategorija(int PodkategorijaID)
        {
            Podkategorija podkategorija = db.Podkategorija.Find(PodkategorijaID);
            if (podkategorija == null)
            {
                return false;
            }
            return true;
        }
        public bool UniqueVjezba(string Naziv)
        {
            Vjezba vjezba = db.Vjezba.Where(x => x.Naziv == Naziv).FirstOrDefault();
            if (vjezba == null)
            {
                return true;
            }
            return false;
        }
        public bool PostojanjeVjezbe(int VjezbaID)
        {
            Vjezba vjezba = db.Vjezba.Find(VjezbaID);
            if (vjezba == null)
            {
                return false;
            }
            return true;
        }
        public IActionResult Email(string Email)
        {
            Korisnik korisnik = db.Korisnik.Where(x => x.Email == Email).FirstOrDefault();
            if(korisnik != null)
            {
                return Json($"Email postoji u bazi!");
            }
            if (IsValidEmail(Email))
            {
                return Json(true);
            }
            return Json($"Neispravan format email-a!");
        }
        public bool UniqueBrojKartice(string BrojKartice)
        {
            Korisnik clan = db.Korisnik.Where(x => x.BrojKartice == BrojKartice).FirstOrDefault();
            if (clan == null)
            {
                return true;
            }
            return false;
        }
        public bool UniqueKategorijaPIP(string Naziv)
        {
            KategorijaPlanIProgram kategorijaPlanIProgram = db.KategorijaPlanIProgram.Where(x => x.Naziv == Naziv && x.Obrisan == false).FirstOrDefault();
            if (kategorijaPlanIProgram == null)
            {
                return true;
            }
            return false;
        }
        public bool IsValidEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
                return false;

            try
            {
                // Normalize the domain
                Email = Regex.Replace(Email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(Email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            
        }
        public bool JMBGValidate(string JMBG)
        {
            if (JMBG != null)
            {
                if (JMBG.Length != 14)
                {
                    return false;
                }
                foreach (char c in JMBG)
                {
                    if (c < '0' || c > '9')
                        return false;
                }
            }
            return true;
        }
    }
    public class MaxFileSize : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSize(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maksimalna velicina slike: { (_maxFileSize/1024)/1024} MB.";
        }
    }
    public class AllowedExtensions : ValidationAttribute
    {
        private readonly string[] _Extensions;
        public AllowedExtensions(string[] Extensions)
        {
            _Extensions = Extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName);
            if (!(file == null))
            {
                if (!_Extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            string msg = "Dozvoljene ekstenzije: ";
            foreach(var x in _Extensions)
            {
                msg += x + ", ";
            }
            return msg;
        }
    }
}