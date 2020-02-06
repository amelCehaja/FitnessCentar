using System;
using System.Collections.Generic;
using System.Linq;
using FitnessCentar.data.EF;
using FitnessCentar.data.Models;
using FitnessCentar.web.ViewModels.Autentifikacija;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessCentar.web.Helper;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace FitnessCentar.web.Controllers
{
    public class AutentifikacijaController : Controller
    {
        private MyContext db;
        public AutentifikacijaController(MyContext myContext)
        {
            db = myContext;
        }
        public IActionResult Index()
        {
            if(HttpContext.GetLogiraniKorisnik() != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginVM()
            {
                ZapamtiPassword = true,
            });
        }
        public bool CheckPassword(byte[] password, byte[] input)
        {
            for (int i = 0; i < 20; i++)
            {
                if (password[i + 16] != input[i])
                {
                    return false;
                }
            }
            return true;
        }
        public IActionResult Login(LoginVM input)
        {
            KorisnickiNalog kn = db.KorisnickiNalog.SingleOrDefault(x => x.KorisnickoIme == input.KorisnickoIme);
            if (kn == null)
            {
                TempData["error_poruka"] = "Pogresan username!";
                return View("Index", input);
            }
            string savedHash = kn.Lozinka;
            byte[] hashBytes = Convert.FromBase64String(savedHash);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            byte[] hash = GetHash(input.Lozinka, salt);

            if (!CheckPassword(hashBytes, hash))
            {
                TempData["error_poruka"] = "Pogresna lozinka!";
                return View("Index", input);
            }

            HttpContext.SetLogiraniKorisnik(kn);
            if (kn.Tip == "admin" || kn.Tip == "zaposlenik")
            {
                return RedirectToAction("PrikazPrisutnihClanova", "AdministracijaClan");
            }
            return RedirectToAction("Index", "Home");

        }
        public List<SelectListItem> GenerateSpolList()
        {
            List<SelectListItem> spol = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text="Muško",
                        Value="M"
                    },
                    new SelectListItem
                    {
                        Text="Žensko",
                        Value="Ž"
                    }
                };
            return spol;
        }
        public IActionResult RegisterForm()
        {
            RegisterVM model = new RegisterVM
            {
                SpolList = GenerateSpolList()
            };
            return View(model);
        }
        public byte[] GetSalt()
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return salt;
        }
        public byte[] GetHash(string input, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, 10000);
            return pbkdf2.GetBytes(20);
        }
        public string GetSavedPasswordHash(byte[] salt, byte[] hash)
        {
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }
        public IActionResult Register(RegisterVM input)
        {
            if (!ModelState.IsValid)
            {
                input.SpolList = GenerateSpolList();
                return View(input);
            }
            //salt
            byte[] salt = GetSalt();
            //hash
            byte[] hash = GetHash(input.Lozinka, salt);
            //salt + hash
            string savedPasswordHash = GetSavedPasswordHash(salt, hash);

            KorisnickiNalog kn = new KorisnickiNalog
            {
                KorisnickoIme = input.KorisnickoIme,
                Lozinka = savedPasswordHash,
                Tip = "clan"
            };
            db.Add(kn);
            db.SaveChanges();

            Korisnik k = new Korisnik
            {
                Ime = input.Ime,
                Prezime = input.Prezime,
                Email = input.Email,
                KorisnickiNalog = kn,
                Spol = input.Spol,
                DatumRodenja = input.DatumRodjenja
            };
            db.Add(k);
            db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout()
        {
            string token = HttpContext.GetTrenutniToken();
            AutorizacijskiToken obrisati = db.AutorizacijskiToken.FirstOrDefault(x => x.Vrijednost == token);
            if (obrisati != null)
            {
                db.AutorizacijskiToken.Remove(obrisati);
                db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ForgotPasswordView()
        {
            return View();
        }
        public void SendVerificationLinkEmail(string username, string emailID, string activationCode, string emailFor = "ResetPassword")
        {
            var verifyUrl = "/Autentifikacija/" + emailFor + "/" + activationCode;
            //for loclahost
            var link = Request.Host.Host + ":" + Request.Host.Port + verifyUrl;

            //for azure
            //var link = Request.Host.Host + verifyUrl;
            var fromEmail = new MailAddress("enterYourMailHere", "Seminarski RS1");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "EnterYourPasswordHere";

            string subject = "";
            string body = "";
            subject = "Reset Password";
            body = "Hi "+ username +",<br/>We got request for reset your account password. Please click on the below link to reset your password" +
                "<br/><br/><a href=\"" + link + "\">" + link + "</a>";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        public IActionResult ForgotPassword(string EmailID,string newUserType = "none")
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            var account = db.Korisnik.Include(x => x.KorisnickiNalog).Where(a => a.Email == EmailID).FirstOrDefault();
            if (account != null)
            {
                //Send email for reset password
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.KorisnickiNalog.KorisnickoIme,account.Email, resetCode, "ResetPassword");
                account.KorisnickiNalog.ResetPasswordCode = resetCode;
                //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                //in our model class in part 1
                db.SaveChanges();
                message = "Reset password link has been sent to your email id.";
            }
            else
            {
                message = "Account not found";
            }
            if(newUserType == "clan")
            {
                return RedirectToAction("AddClanPhoto","AdministracijaClan");
            }
            else if(newUserType == "zaposlenik")
            {
                return RedirectToAction("PrikazZaposlenika", "AdministracijaZaposlenik");
            }

            ViewBag.Message = message;
            return View("ForgotPasswordView");
        }
        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var user = db.Korisnik.Where(a => a.KorisnickiNalog.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return null;
            }
        }
        public ActionResult SetNewPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var user = db.KorisnickiNalog.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    byte[] salt = GetSalt();
                    //hash
                    byte[] hash = GetHash(model.NewPassword, salt);
                    //salt + hash
                    string savedPasswordHash = GetSavedPasswordHash(salt, hash);
                    user.Lozinka = savedPasswordHash;
                    user.ResetPasswordCode = "";
                    db.SaveChanges();
                    message = "New password updated successfully";
                }
            }
            else
            {
                message = "Something invalid";
                ViewBag.Message = message;
                return View("ResetPassword",model);
            }
            ViewBag.Message = message;
            return View("Index");
        }
    }
}
