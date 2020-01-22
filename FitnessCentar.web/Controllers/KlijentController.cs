using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessCentar.data.EF;
using FitnessCentar.web.Helper;
using FitnessCentar.web.ViewModels.Klijent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessCentar.web.Controllers
{
    public class KlijentController : Controller
    {
        private MyContext db;
        public KlijentController(MyContext myContext)
        {
            db = myContext;
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

        public List<SelectListItem> GenerateAktivnostList()
        {
            List<SelectListItem> aktivnost = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text="Neaktivan/Neaktivna (ne vježbam)",
                        Value="1,2"
                    },
                    new SelectListItem
                    {
                        Text="Slabo aktivan/aktivna (lahke vježbe/sportovi 1-3 dana u sedmici)",
                        Value="1,375"
                    },
                    new SelectListItem
                    {
                        Text="Umjereno aktivan/aktivna (umjerene vježbe/sportovi 3-5 dana u sedmici)",
                        Value="1,55"
                    },
                    new SelectListItem
                    {
                        Text="Veoma aktivan/aktivna (teške vježbe/sportovi 6-7 dana u sedmici)",
                        Value="1,725"
                    },
                    new SelectListItem
                    {
                        Text="Atletičar/Atletičarka (veoma teške vježbe/fizički posao/trening 2 puta po danu)",
                        Value="1,9"
                    }
                };
            return aktivnost;
        }

        public List<SelectListItem> GenerateBmiKategorije()
        {
            List<SelectListItem> bmiKategorije = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Ispod normalne težine",
                    Value="Manje od 18,5"
                },
                new SelectListItem
                {
                    Text = "Normalna težina",
                    Value="18,5 - 24,9"
                },
                new SelectListItem
                {
                    Text = "Iznad normalne težine",
                    Value="25-29,9"
                },
                new SelectListItem
                {
                    Text = "Gojazan",
                    Value="30+"
                }
            };
            return bmiKategorije;
        }
        public double CalculateTdee(KalkulatorVM kalkulator)
        {
            double tdee, bmr;

            if (kalkulator.UdioMasnoce != null)
            {
                double lbm = kalkulator.Tezina - (kalkulator.Tezina * (double)kalkulator.UdioMasnoce / 100);
                bmr = 370 + (21.6 * lbm);
            }
            else
            {
                bmr = (10 * kalkulator.Tezina) + (6.25 * kalkulator.Visina) - (5 * kalkulator.Starost);

                if (kalkulator.Spol == "M")
                {
                    bmr += 5;
                }

                if (kalkulator.Spol == "Ž")
                {
                    bmr -= 161;
                }

            }
            tdee = Math.Round(bmr * double.Parse(kalkulator.Aktivnost));

            return tdee;
        }

        public double CalculateBMI(int tezina, int visina)
        {
            double bmi = Math.Round(tezina / Math.Pow((double)visina / 100, 2), 1);
            return bmi;
        }

        public IActionResult KalkulatorKalorija()
        {
            KalkulatorVM model = new KalkulatorVM
            {
                SpolList = GenerateSpolList(),
                AktivnostList = GenerateAktivnostList()
            };

            return View(model);
        }

        public IActionResult Izracunaj(KalkulatorVM kalkulator)
        {
            KalkulatorRezultatVM model = new KalkulatorRezultatVM
            {
                Tdee = CalculateTdee(kalkulator),
                Bmi = CalculateBMI(kalkulator.Tezina, kalkulator.Visina),
                Starost = kalkulator.Starost,
                Tezina = kalkulator.Tezina,
                Visina = kalkulator.Visina,
                UdioMasnoce = kalkulator.UdioMasnoce,
                BmiKategorijaList = GenerateBmiKategorije()
            };
            if (model.Bmi < 18.5)
            {
                model.BmiKategorija = "Ispod normalne težine";
            }
            else if (model.Bmi >= 18.5 && model.Bmi <= 24.9)
            {
                model.BmiKategorija = "Normalna težina";
            }
            else if (model.Bmi >= 25 && model.Bmi <= 29.9)
            {
                model.BmiKategorija = "Iznad normalne težine";
            }
            else
            {
                model.BmiKategorija = "Gojazan";
            }


            return View(model);
        }
    }
}