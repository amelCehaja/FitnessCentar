using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Controllers;
using FitnessCentar.web.Helpers;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FitnessCentar.test.AdministracijaTests
{
    public class AdministracijaZaposlenikUnitTests
    {
        private Mock<IZaposlenikService> _service;
        private Mock<IClanService> _clanService;
        private Mock<Adminsitracija_Helper> _helper;
        private AdministracijaZaposlenikController _controller;
        public AdministracijaZaposlenikUnitTests()
        {
            _service = new Mock<IZaposlenikService>();
            _helper = new Mock<Adminsitracija_Helper>();
            _clanService = new Mock<IClanService>();
            _controller = new AdministracijaZaposlenikController(_service.Object, _helper.Object,_clanService.Object);
        }
        [Fact]
        public void DodajZaposlenika_ReturnsViewWithModel()
        {
            var result = _controller.DodajZaposlenika() as ViewResult;
            var model = Assert.IsType<AdministracijaDodajZaposlenikaVM>(result.Model);
            Assert.Equal("DodajZaposlenika", result.ViewName);
        }
        [Fact]
        public void SpremiZaposlenika_BadModel()
        {            
            AdministracijaDodajZaposlenikaVM zaposlenik = new AdministracijaDodajZaposlenikaVM
            {
                Prezime = "Prezime",
                JMBG = "123123123"
            };
            _controller.ModelState.AddModelError("Naziv", "Naziv is required!");
            var result = _controller.SpremiZaposlenika(zaposlenik) as ViewResult;
            var model = Assert.IsType<AdministracijaDodajZaposlenikaVM>(result.Model);
            Assert.Equal(zaposlenik.Prezime, model.Prezime);
            Assert.Equal(zaposlenik.JMBG, model.JMBG);
            Assert.Equal("DodajZaposlenika", result.ViewName);
            _service.Verify(x => x.DodajZaposlenika(It.IsAny<Korisnik>()), Times.Never);
        }
        [Fact]
        public void SpremiZaposlenik_GoodModel()
        {
            var result = _controller.SpremiZaposlenika(new AdministracijaDodajZaposlenikaVM()) as RedirectToActionResult;
            Assert.Equal("PrikazZaposlenika", result.ActionName);
            _service.Verify(x => x.DodajZaposlenika(It.IsAny<Korisnik>()), Times.Once);
        }
        [Fact]
        public void ObrisiZaposlenika_DoesntExistInDB()
        {
            _service.Setup(x => x.ZaposlenikFind(It.IsAny<int>())).Returns(null as Korisnik);
            var result = _controller.ObrisiZaposlenika(It.IsAny<int>()) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
            _service.Verify(x => x.ObrisiZaposlenik(It.IsAny<Korisnik>()), Times.Never);
        }
        [Fact]
        public void ObrisiZaposlenika_ExistsInDB()
        {
            _service.Setup(x => x.ZaposlenikFind(It.IsAny<int>())).Returns(new Korisnik());
            var result = _controller.ObrisiZaposlenika(It.IsAny<int>()) as RedirectToActionResult;
            Assert.Equal("PrikazZaposlenika", result.ActionName);
            _service.Verify(x => x.ObrisiZaposlenik(It.IsAny<Korisnik>()), Times.Once);
        }
        [Fact]
        public void PrikazZaposlenika_ReturnViewWithModel()
        {
            List<Korisnik> zaposlenici = new List<Korisnik> { new Korisnik(), new Korisnik(), new Korisnik(), new Korisnik() };
            _service.Setup(x => x.GetKorisnike()).Returns(zaposlenici);
            var result = _controller.PrikazZaposlenika() as ViewResult;
            var model = Assert.IsType<AdministracijPrikazZaposlenikaVM>(result.Model);
            Assert.Equal(zaposlenici.Count, model.zaposlenici.Count);
            Assert.Equal("PrikazZaposlenika", result.ViewName);
        }
    }
}
