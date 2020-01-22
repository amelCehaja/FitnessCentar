using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Controllers;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using FitnessCentar.web.ViewModels.AjaxVMs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FitnessCentar.test.AdministracijaTests
{
    public class AjaxUnitTests
    {
        Mock<IAjaxService> _service;
        Mock<IClanService> _clanService;
        Mock<IPlanIProgramService> _planService;
        Mock<IWebShopService> _webShopService;
        AjaxController _controller;
        public AjaxUnitTests()
        {
            _service = new Mock<IAjaxService>();
            _clanService = new Mock<IClanService>();
            _planService = new Mock<IPlanIProgramService>();
            _webShopService = new Mock<IWebShopService>();
            _controller = new AjaxController(_service.Object,_clanService.Object,_planService.Object,_webShopService.Object);
        }
        [Fact]
        public void TrajanjeClanarine_ReturnsInt()
        {
            int trajanje = 20;
            _service.Setup(x => x.TrajanjeTipaClanarine(It.IsAny<int>())).Returns(trajanje);
            var result = _controller.TrajanjeClanarine(1);
            Assert.Equal(trajanje, result);
        }
        [Fact]
        public void PrikazClanarina_ReturnsViewWithModel()
        {
            List<Clanarina> clanarine = new List<Clanarina> { new Clanarina { TipClanarine = new TipClanarine {Naziv = "tip" }, Clan = new Korisnik() }, new Clanarina { TipClanarine = new TipClanarine { Naziv = "tip" }, Clan = new Korisnik()}, new Clanarina { TipClanarine = new TipClanarine(), Clan = new Korisnik() } };
            _service.Setup(x => x.GetClanarineByStatus(It.IsAny<string>())).Returns(clanarine);
            var result = _controller.PrikazClanarina("tip", "sve") as PartialViewResult;
            var model = Assert.IsType<AjaxPrikazClanarinaVM>(result.Model);
            Assert.Equal(2, model.clanovi.Count);
            Assert.Equal("PrikazClanarina", result.ViewName);
        }
        [Fact]
        public void PrikazClanova_Sve()
        {
            List<Korisnik> clanarine = new List<Korisnik> { new Korisnik(), new Korisnik(), new Korisnik(), new Korisnik() };
            _service.Setup(x => x.PrikazClanovaPoImenu(It.IsAny<string>())).Returns(clanarine);
            _clanService.Setup(x => x.AktivnaClanarina(It.IsAny<int>())).Returns(null as Clanarina);
            var result = _controller.PrikazClanova(null,null) as PartialViewResult;
            var model = Assert.IsType<AjaxPrikazClanova>(result.Model);
            Assert.Equal(clanarine.Count, model.clanovi.Count);
            Assert.Equal("PrikazClanova", result.ViewName);
        }
        [Fact]
        public void PrikazSedmice_ReturnsPartialViewWithModel()
        {
            List<Dan> dani = new List<Dan> { new Dan { }, new Dan { }, new Dan { } };
            _service.Setup(x => x.GetDaniBySedmicaID(It.IsAny<int>())).Returns(dani);
            var result = _controller.PrikazSedmice(null, 1) as PartialViewResult;
            var model = Assert.IsType<AjaxPrikazSedmiceVM>(result.Model);
            Assert.Equal(dani.Count, model.Dani.Count);
            Assert.Equal("PrikazSedmice", result.ViewName);
        }
        [Fact]
        public void PrikazDana_ReturnsPartialViewWithModel()
        {
            List<DanVjezba> danVjezbe = new List<DanVjezba> { new DanVjezba { Vjezba = new Vjezba()}, new DanVjezba { Vjezba = new Vjezba() }, new DanVjezba { Vjezba = new Vjezba() } };
            _service.Setup(x => x.GetDanVjezbeOrderdByRedniBroj(It.IsAny<int>())).Returns(danVjezbe);
            var result = _controller.PrikazDana(null, 1) as PartialViewResult;
            var model = Assert.IsType<AjaxPrikazDanaVM>(result.Model);
            Assert.Equal(danVjezbe.Count, model.Vjezbe.Count);
            Assert.Equal("PrikazDana", result.ViewName);
        }
        [Fact]
        public void ObrisiDanVjezba_DoesntExistInDB()
        {
            _service.Setup(x => x.DanVjezbaFind(It.IsAny<int>())).Returns(null as DanVjezba);
            var result = _controller.ObrisiDanVjezba(1) as PartialViewResult;
            Assert.Equal("NotFound", result.ViewName);
            _service.Verify(x => x.RemoveDanVjezba(It.IsAny<DanVjezba>()), Times.Never);
        }
        [Fact]
        public void ObrisiDanVjezba_ExistsInDB()
        {
            _service.Setup(x => x.DanVjezbaFind(It.IsAny<int>())).Returns(new DanVjezba());
            var result = _controller.ObrisiDanVjezba(1) as RedirectToActionResult;
            Assert.Equal("PrikazDana", result.ActionName);
            _service.Verify(x => x.RemoveDanVjezba(It.IsAny<DanVjezba>()), Times.Once);
        }
        [Fact]
        public void DodajDanVjezba_ReturnsPartialViewWithModel()
        {
            List<Vjezba> vjezbe = new List<Vjezba> { new Vjezba(), new Vjezba(), new Vjezba(), new Vjezba() };
            List<DanVjezba> danVjezbe = new List<DanVjezba> { new DanVjezba(), new DanVjezba(), new DanVjezba() };
            _service.Setup(x => x.getVjezbe()).Returns(vjezbe);
            _service.Setup(x => x.GetDanVjezbeOrderdByRedniBroj(It.IsAny<int>())).Returns(danVjezbe);
            var result = _controller.DodajDanVjezba(1) as PartialViewResult;
            var model = Assert.IsType<AjaxDodajDanVjezbaVM>(result.Model);
            Assert.Equal(vjezbe.Count, model.Vjezbe.Count);
            Assert.Equal(danVjezbe.Count + 1, model.RedniBrojevi.Count);
            Assert.Equal("DodajDanVjezba", result.ViewName);
        }
        [Fact]
        public void SpremiDanVjezba_BadModel()
        {
            _controller.ModelState.AddModelError("Vjezba", "Vjezba is incorrect!");
            List<Vjezba> vjezbe = new List<Vjezba> { new Vjezba(), new Vjezba(), new Vjezba(), new Vjezba() };
            List<DanVjezba> danVjezbe = new List<DanVjezba> { new DanVjezba(), new DanVjezba(), new DanVjezba() };
            _service.Setup(x => x.getVjezbe()).Returns(vjezbe);
            _service.Setup(x => x.GetDanVjezbeOrderdByRedniBroj(It.IsAny<int>())).Returns(danVjezbe);
            var result = _controller.SpremiDanVjezba(new AjaxDodajDanVjezbaVM()) as PartialViewResult;
            var model = Assert.IsType<AjaxDodajDanVjezbaVM>(result.Model);
            Assert.Equal(vjezbe.Count, model.Vjezbe.Count);
            Assert.Equal(danVjezbe.Count + 1, model.RedniBrojevi.Count);
            Assert.Equal("DodajDanVjezba", result.ViewName);
            _service.Verify(x => x.DodajDanVjezba(It.IsAny<DanVjezba>()), Times.Never);
            _service.Verify(x => x.IncreaseRedniBroj(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
        [Fact]
        public void SpremiDanVjezba_GoodModel()
        {
            var result = _controller.SpremiDanVjezba(new AjaxDodajDanVjezbaVM()) as RedirectToActionResult;      
            Assert.Equal("PrikazDana", result.ActionName);
            _service.Verify(x => x.DodajDanVjezba(It.IsAny<DanVjezba>()), Times.Once);
            _service.Verify(x => x.IncreaseRedniBroj(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public void UreditDanVjezba_DoesntExistInDB()
        {
            _service.Setup(x => x.DanVjezbaFind(It.IsAny<int>())).Returns(null as DanVjezba);
            var result = _controller.UrediDanVjezba(1) as PartialViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void UrediDanVjezba_ExistsInDB()
        {
            List<DanVjezba> danVjezbe = new List<DanVjezba> { new DanVjezba(), new DanVjezba(), new DanVjezba() };
            _service.Setup(x => x.DanVjezbaFind(It.IsAny<int>())).Returns(new DanVjezba { Vjezba = new Vjezba() });
            _service.Setup(x => x.GetDanVjezbeOrderdByRedniBroj(It.IsAny<int>())).Returns(danVjezbe);
            var result = _controller.UrediDanVjezba(1) as PartialViewResult;
            var model = Assert.IsType<AjaxUrediDanVjezbaVM>(result.Model);
            Assert.Equal(danVjezbe.Count, model.RedniBrojevi.Count);
            Assert.Equal("UrediDanVjezba", result.ViewName);
        }
        [Fact]
        public void SpremiUrediDan_BadModel()
        {
            List<DanVjezba> danVjezbe = new List<DanVjezba> { new DanVjezba(), new DanVjezba(), new DanVjezba() };
            _controller.ModelState.AddModelError("BrojPonavljanja", "Broj poanvaljanja ne moze biti 0!");
            _service.Setup(x => x.GetDanVjezbeOrderdByRedniBroj(It.IsAny<int>())).Returns(danVjezbe);
            var result = _controller.SpremiUrediDanVjezba(new AjaxUrediDanVjezbaVM()) as PartialViewResult;
            var model = Assert.IsType<AjaxUrediDanVjezbaVM>(result.Model);
            Assert.Equal(danVjezbe.Count, model.RedniBrojevi.Count);
            Assert.Equal("UrediDanVjezba", result.ViewName);
            _service.Verify(x => x.UpdateDanVjezba(It.IsAny<DanVjezba>()),Times.Never);
            _service.Verify(x => x.ChangeRedniBroj(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()),Times.Never);
        }
        [Fact]
        public void SpremiUrediDan_GoodModel()
        {
            _service.Setup(x => x.DanVjezbaFind(It.IsAny<int>())).Returns(new DanVjezba());
            var result = _controller.SpremiUrediDanVjezba(new AjaxUrediDanVjezbaVM()) as RedirectToActionResult;
            Assert.Equal("PrikazDana", result.ActionName);
            _service.Verify(x => x.UpdateDanVjezba(It.IsAny<DanVjezba>()), Times.Once);
            _service.Verify(x => x.ChangeRedniBroj(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public void PrisutsvtoClana_DoesntExistInDB()
        {
            _service.Setup(x => x.GetClanByBrojKartice(It.IsAny<string>())).Returns(null as Korisnik);
            var result = _controller.PrisutstvoClana("123") as RedirectToActionResult;
            _service.Verify(x => x.DodajPosjecenostClana(It.IsAny<PosjecenostClana>()), Times.Never);
            _service.Verify(x => x.UpdatePosjecenostClana(It.IsAny<PosjecenostClana>()), Times.Never);
            Assert.Equal("PrikazPrisutnihClanova", result.ActionName);
        }
        [Fact]
        public void PrisutsvtoClana_ExistSInDB_PosjecenostClanaNULL()
        {
            _service.Setup(x => x.GetClanByBrojKartice(It.IsAny<string>())).Returns(new Korisnik());
            _service.Setup(x => x.GetPosjecenostClanaByBrojKartice(It.IsAny<string>())).Returns(null as PosjecenostClana);
            var result = _controller.PrisutstvoClana("123") as RedirectToActionResult;
            _service.Verify(x => x.DodajPosjecenostClana(It.IsAny<PosjecenostClana>()), Times.Once);
            _service.Verify(x => x.UpdatePosjecenostClana(It.IsAny<PosjecenostClana>()), Times.Never);
            Assert.Equal("PrikazPrisutnihClanova", result.ActionName);
        }
        [Fact]
        public void PrisutsvtoClana_ExistSInDB_PosjecenostClanaNotNULL()
        {
            _service.Setup(x => x.GetClanByBrojKartice(It.IsAny<string>())).Returns(new Korisnik());
            _service.Setup(x => x.GetPosjecenostClanaByBrojKartice(It.IsAny<string>())).Returns(new PosjecenostClana());
            var result = _controller.PrisutstvoClana("123") as RedirectToActionResult;
            _service.Verify(x => x.DodajPosjecenostClana(It.IsAny<PosjecenostClana>()), Times.Never);
            _service.Verify(x => x.UpdatePosjecenostClana(It.IsAny<PosjecenostClana>()), Times.Once);
            Assert.Equal("PrikazPrisutnihClanova", result.ActionName);
        }
        [Fact]
        public void PrikazPrisutnihClanova_ClanIDNULL()
        {
            List<PosjecenostClana> posjecenostClana = new List<PosjecenostClana> { new PosjecenostClana { Clan = new Korisnik() }, new PosjecenostClana { Clan = new Korisnik() }, new PosjecenostClana { Clan = new Korisnik() } };
            _service.Setup(x => x.GetPosjecenostClana()).Returns(posjecenostClana);
            var result = _controller.PrikazPrisutnihClanova() as PartialViewResult;
            var model = Assert.IsType<AdministracijaPrikazPrisutnihClanovaVM>(result.Model);
            Assert.Equal(posjecenostClana.Count, model.Clanovi.Count);
            _service.Verify(x => x.GetClanByID(It.IsAny<int>()), Times.Never);
            Assert.Equal("PrikazPrisutnihClanova", result.ViewName);
        }
        [Fact]
        public void PrikazPrisutnihClanova_ClanIDNotNULL()
        {
            List<PosjecenostClana> posjecenostClana = new List<PosjecenostClana> { new PosjecenostClana { Clan = new Korisnik() }, new PosjecenostClana { Clan = new Korisnik() }, new PosjecenostClana { Clan = new Korisnik() } };
            _service.Setup(x => x.GetPosjecenostClana()).Returns(posjecenostClana);
            _service.Setup(x => x.GetClanByID(It.IsAny<int>())).Returns(new Korisnik());
            var result = _controller.PrikazPrisutnihClanova() as PartialViewResult;
            var model = Assert.IsType<AdministracijaPrikazPrisutnihClanovaVM>(result.Model);
            Assert.Equal(posjecenostClana.Count, model.Clanovi.Count);
            _service.Verify(x => x.GetClanByID(It.IsAny<int>()), Times.Once);
            Assert.Equal("PrikazPrisutnihClanova", result.ViewName);
        }
    }
}
