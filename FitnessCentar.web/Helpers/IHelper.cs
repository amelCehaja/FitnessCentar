using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCentar.web.Helpers
{
    public interface IHelper
    {
        List<SelectListItem> GenereateSpolList();
        List<SelectListItem> ClanarineAN();
        byte[] GetSalt();
        byte[] GetHash(string input, byte[] salt);
        string GetSavedPasswordHash(byte[] salt, byte[] hash);
    }
}
