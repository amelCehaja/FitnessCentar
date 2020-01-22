using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Controllers;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FitnessCentar.test.AdministracijaTests
{
    public class AdministracijaWebShopUnitTests
    {
        private Mock<IWebShopService> _service;
        private Mock<IHostingEnvironment> _environment;
        private AdministracijaWebShopController _controller;
        public AdministracijaWebShopUnitTests()
        {
            _service = new Mock<IWebShopService>();
            _environment = new Mock<IHostingEnvironment>();
            _controller = new AdministracijaWebShopController(_service.Object,_environment.Object);
        }
        [Fact]
        public void PrikazKategorija_ReturnsViewWithModel()
        {
            List<Kategorija> kategorije = new List<Kategorija> { new Kategorija(), new Kategorija(), new Kategorija() };
            _service.Setup(x => x.GetKategorije()).Returns(kategorije);
            var result = _controller.PrikazKategorija() as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazKategorijaVM>(result.Model);
            Assert.Equal(kategorije.Count, model.Kategorije.Count);
            Assert.Equal("PrikazKategorija", result.ViewName);
        }
        [Fact]
        public void DodajKategoriju_ReturnsViewWithModel()
        {
            var result = _controller.DodajKategoriju() as ViewResult;
            Assert.IsType<AdministracijaDodajKategorijuVM>(result.Model);
            Assert.Equal("DodajKategoriju", result.ViewName);
        }
        [Fact]
        public void SpremiKategoriju_BadModel()
        {
            _controller.ModelState.AddModelError("Naziv", "Naziv is required!");
            var result = _controller.SpremiKategoriju(new AdministracijaDodajKategorijuVM()) as ViewResult;
            Assert.IsType<AdministracijaDodajKategorijuVM>(result.Model);
            Assert.Equal("DodajKategoriju", result.ViewName);
            _service.Verify(x => x.DodajKategoriju(It.IsAny<Kategorija>()), Times.Never);
        }
        [Fact]
        public void SpremiKategoriju_GoodModel()
        {
            AdministracijaDodajKategorijuVM dodajKategorijuVM = new AdministracijaDodajKategorijuVM { Naziv = "Kategorija" };
            var result = _controller.SpremiKategoriju(dodajKategorijuVM) as RedirectToActionResult;
            _service.Verify(x => x.DodajKategoriju(It.IsAny<Kategorija>()), Times.Once);
            Assert.Equal("PrikazKategorija", result.ActionName);
        }
        [Fact]
        public void DodajStavku_ReturnsViewWithModel()
        {
            List<SelectListItem> kategorije = new List<SelectListItem> { new SelectListItem(), new SelectListItem() };
            _service.Setup(x => x.GetKategorijeSelectListItem()).Returns(kategorije);
            var result = _controller.DodajStavku() as ViewResult;
            var model = Assert.IsType<AdministracijaDodajStavkuVM>(result.Model);
            Assert.Equal(kategorije.Count, model.Kategorije.Count);
            Assert.Equal("DodajStavku", result.ViewName);
        }
        [Fact]
        public void SpremiStavku_BadModel()
        {
            _controller.ModelState.AddModelError("Naziv", "Naziv is required!");
            List<SelectListItem> kategorije = new List<SelectListItem> { new SelectListItem(), new SelectListItem() };
            _service.Setup(x => x.GetKategorijeSelectListItem()).Returns(kategorije);
            var result = _controller.SpremiStavku(new AdministracijaDodajStavkuVM()) as ViewResult;
            var model = Assert.IsType<AdministracijaDodajStavkuVM>(result.Model);
            Assert.Equal(kategorije.Count, model.Kategorije.Count);
            Assert.Equal("DodajStavku", result.ViewName);
            _service.Verify(x => x.DodajStavku(It.IsAny<Stavka>()), Times.Never);
        }
        [Fact]
        public void SpremiStavku_GoodModel()
        {
            var result = _controller.SpremiStavku(new AdministracijaDodajStavkuVM()) as RedirectToActionResult;
            _service.Verify(x => x.DodajStavku(It.IsAny<Stavka>()), Times.Once);
            Assert.Equal("PrikazStavke", result.ActionName);
        }
        [Fact]
        public void PrikazStavke_ReturnsViewWithModel()
        {
            List<Stavka> stavke = new List<Stavka> { new Stavka { Podkategorija = new Podkategorija { Kategorija = new Kategorija() } }, new Stavka { Podkategorija = new Podkategorija { Kategorija = new Kategorija() } }, new Stavka { Podkategorija = new Podkategorija { Kategorija = new Kategorija() } } };
            _service.Setup(x => x.GetStavke()).Returns(stavke);
            var result = _controller.PrikazStavke("") as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazStavke>(result.Model);
            Assert.Equal(stavke.Count, model.Stavke.Count);
            Assert.Equal("PrikazStavke", result.ViewName);
        }
        [Fact]
        public void UrediStavku_DoesntExistInDB()
        {
            _service.Setup(x => x.StavkaFind(It.IsAny<int>())).Returns(null as Stavka);
            var result = _controller.UrediStavku(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void UrediStavku_ExistsInDB()
        {
            Stavka stavka = new Stavka
            {
                ID = 1,
                Naziv = "Naziv",
                Cijena = 20
            };
            _service.Setup(x => x.StavkaFind(It.IsAny<int>())).Returns(stavka);
            var result = _controller.UrediStavku(1) as ViewResult;
            var model = Assert.IsType<AdministracijaUrediStavkuVM>(result.Model);
            Assert.Equal(stavka.ID, model.ID);
            Assert.Equal(stavka.Naziv, model.Naziv);
            Assert.Equal(stavka.Cijena, model.Cijena);
            Assert.Equal("UrediStavku", result.ViewName);
        }
        [Fact]
        public void SpremiUrediStavku_BadModel()
        {
            _controller.ModelState.AddModelError("Naziv", "Naziv is required!");
            AdministracijaUrediStavkuVM urediStavkuVM = new AdministracijaUrediStavkuVM
            {
                ID = 1,
                Cijena = 20
            };
            var result = _controller.SpremiUrediStavku(urediStavkuVM) as ViewResult;
            var model = Assert.IsType<AdministracijaUrediStavkuVM>(result.Model);
            Assert.Equal(urediStavkuVM.ID, model.ID);
            Assert.Equal(urediStavkuVM.Cijena, model.Cijena);
            Assert.Equal("UrediStavku", result.ViewName);
            _service.Verify(x => x.UpdateStavka(It.IsAny<Stavka>()), Times.Never);
        }
        [Fact]
        public void SpremiUrediStavku_GoodModel()
        {
            _service.Setup(x => x.StavkaFind(It.IsAny<int>())).Returns(new Stavka());
            var result = _controller.SpremiUrediStavku(new AdministracijaUrediStavkuVM()) as RedirectToActionResult;
            Assert.Equal("PrikazStavke",result.ActionName);
            _service.Verify(x => x.UpdateStavka(It.IsAny<Stavka>()), Times.Once);
        }
        [Fact]
        public void ObrisiStavku_DoesntExistInDB()
        {
            _service.Setup(x => x.StavkaFind(It.IsAny<int>())).Returns(null as Stavka);
            var result = _controller.ObrisiStavku(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
            _service.Verify(x => x.UpdateStavka(It.IsAny<Stavka>()), Times.Never);
        }
        [Fact]
        public void ObrisiStavku_ExistsInDB()
        {
            _service.Setup(x => x.StavkaFind(It.IsAny<int>())).Returns(new Stavka());
            var result = _controller.ObrisiStavku(2) as RedirectToActionResult;
            Assert.Equal("PrikazStavke", result.ActionName);
            _service.Verify(x => x.UpdateStavka(It.IsAny<Stavka>()), Times.Once);
        }
        [Fact]
        public void PrikazNarudzbi_ReturnsViewWithModel()
        {
            List<Narudzba> narudzbe = new List<Narudzba> { new Narudzba(), new Narudzba(), new Narudzba() };
            _service.Setup(x => x.GetNarudzbe()).Returns(narudzbe);
            var result = _controller.PrikazNarudzbi() as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazNaruzbiVM>(result.Model);
            Assert.Equal(narudzbe.Count, model.Narudzbe.Count);
            Assert.Equal("PrikazNarudzbi", result.ViewName);
        }
        [Fact]
        public void DetaljiNarudzba_DoesntExistInDB()
        {
            _service.Setup(x => x.NarudzbaFind(It.IsAny<int>())).Returns(null as Narudzba);
            var result = _controller.DetaljiNarudzba(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void DetaljiNarudzba_ExistsInDB()
        {
            List<NarudzbaStavke> narudzbaStavke = new List<NarudzbaStavke> { new NarudzbaStavke { Stavka = new Stavka { Podkategorija = new Podkategorija { Kategorija = new Kategorija() } } } };
            _service.Setup(x => x.NarudzbaFind(It.IsAny<int>())).Returns(new Narudzba { Adresa = new Adresa(), Korisnik = new Korisnik() });
            _service.Setup(x => x.GetNarudzbaStavke(It.IsAny<int>())).Returns(narudzbaStavke);
            var result = _controller.DetaljiNarudzba(1) as ViewResult;
            var model = Assert.IsType<AdministracijaDetaljiNarudzbaVM>(result.Model);
            Assert.Equal(narudzbaStavke.Count, model.Stavke.Count);
            Assert.Equal("DetaljiNarudzba", result.ViewName);
        }
        [Fact]
        public void Isporuceno_DoesntExistInDB()
        {
            _service.Setup(x => x.NarudzbaFind(It.IsAny<int>())).Returns(null as Narudzba);
            var result = _controller.Isporuceno(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
            _service.Verify(x => x.UpdateNarudzba(It.IsAny<Narudzba>()), Times.Never);
            _service.Verify(x => x.DodajRacun(It.IsAny<Racun>()), Times.Never);
        }
        [Fact]
        public void Isporuceno_ExistsInDB()
        {
            _service.Setup(x => x.NarudzbaFind(It.IsAny<int>())).Returns(new Narudzba());
            var result = _controller.Isporuceno(1) as RedirectToActionResult;
            Assert.Equal("DetaljiNarudzba", result.ActionName);
            _service.Verify(x => x.UpdateNarudzba(It.IsAny<Narudzba>()), Times.Once);
            _service.Verify(x => x.DodajRacun(It.IsAny<Racun>()), Times.Once);
        }
        [Fact]
        public void PrikazRacuna()
        {
            List<Racun> racuni = new List<Racun> { new Racun { }, new Racun { }, new Racun { } };
            _service.Setup(x => x.GetRacune()).Returns(racuni);
            var result = _controller.PrikazRacuna() as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazRacunaVM>(result.Model);
            Assert.Equal(racuni.Count, model.Racuni.Count);
            Assert.Equal("PrikazRacuna", result.ViewName);
        }
    }
}
