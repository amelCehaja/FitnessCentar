using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessCentar.web.Helpers
{
    public class Adminsitracija_Helper : IHelper
    {
        public List<SelectListItem> GenereateSpolList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Muško",
                    Value = "M"
                },
                new SelectListItem
                {
                    Text = "Žensko",
                    Value = "Ž"
                }
            };
        }
        public List<SelectListItem> ClanarineAN()
        {
            return new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Sve",
                    Value = "sve",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Aktivne",
                    Value = "da"
                },
                new SelectListItem
                {
                    Text = "Neaktivne",
                    Value = "ne"
                }
            };
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
    }
}
