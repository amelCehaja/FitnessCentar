using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Controllers;
using FitnessCentar.web.Helpers;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace FitnessCentar.test.AdministracijaTests
{
    public class AdministracijaClanUnitTests
    {
        private readonly Mock<IClanService> _repository;
        private readonly Mock<IHelper> _helper;
        private readonly Mock<IHostingEnvironment> _hosting;
        private readonly AdministracijaClanController _controller;

        public AdministracijaClanUnitTests()
        {
            _repository = new Mock<IClanService>();
            _helper = new Mock<IHelper>();
            _hosting = new Mock<IHostingEnvironment>();
            _controller = new AdministracijaClanController(_repository.Object, _helper.Object, _hosting.Object);
        }

        [Fact]
        public void DodajClana_ReturnsView_WithModel()
        {
            _repository.Setup(x => x.GetTipoviClanarineSelectList()).Returns(new List<SelectListItem>() { new SelectListItem(), new SelectListItem() });
            var result = _controller.DodajClana() as ViewResult;
            var model = Assert.IsType<AdministracijaDodajClanaVM>(result.Model);
            Assert.Equal(2, model.clanarine.Count);
        }
        [Fact]
        public void SpremiClana_BadModel()
        {
            AdministracijaDodajClanaVM model = new AdministracijaDodajClanaVM
            {
                Ime = "Ime",
                Prezime = "Prezime",
                TipClanarineID = 2,
                DatumDodavanja = new DateTime(2019, 2, 2),
                DatumIsteka = new DateTime(2019, 3, 3)
            };
            _controller.ModelState.AddModelError("BrojKartice", "BrojKartice is required");
            var result = _controller.SpremiClan(model) as ViewResult;

            _repository.Verify(x => x.DodajClana(It.IsAny<Korisnik>()), Times.Never);
            _repository.Verify(x => x.DodajClanarinu(It.IsAny<Clanarina>()), Times.Never);

            var returnedModel = result.Model as AdministracijaDodajClanaVM;
            Assert.Equal(model.Ime, returnedModel.Ime);
            Assert.Equal(model.Prezime, returnedModel.Prezime);
            Assert.Equal(model.TipClanarineID, returnedModel.TipClanarineID);
            Assert.Equal(model.DatumDodavanja, returnedModel.DatumDodavanja);
            Assert.Equal(model.DatumIsteka, returnedModel.DatumIsteka);
            Assert.Equal("DodajClanaForma", result.ViewName);
        }
        [Fact]
        public void SpremiClana_GoodModel()
        {
            AdministracijaDodajClanaVM model = new AdministracijaDodajClanaVM
            {
                Ime = "Ime",
                Prezime = "Prezime",
                TipClanarineID = 2,
                DatumDodavanja = new DateTime(2019, 2, 2),
                DatumIsteka = new DateTime(2019, 3, 3)
            };
            Korisnik clan = null;
            _repository.Setup(r => r.DodajClana(It.IsAny<Korisnik>())).Callback<Korisnik>(x => clan = x);
            Clanarina clanarina = null;
            _repository.Setup(r => r.DodajClanarinu(It.IsAny<Clanarina>())).Callback<Clanarina>(x => clanarina = x);

            var result = _controller.SpremiClan(model) as RedirectToActionResult;
            _repository.Verify(x => x.DodajClana(It.IsAny<Korisnik>()), Times.Once);
            _repository.Verify(x => x.DodajClanarinu(It.IsAny<Clanarina>()), Times.Once);

            Assert.Equal(model.Ime, clan.Ime);
            Assert.Equal(model.Prezime, clan.Prezime);
            Assert.Equal(model.TipClanarineID, clanarina.TipClanarineID);
            Assert.Equal(model.DatumDodavanja, clanarina.DatumDodavanja);
            Assert.Equal(model.DatumIsteka, clanarina.DatumIsteka);
            Assert.Equal("AddClanPhoto", result.ActionName);
        }
        [Fact]
        public void EditClan_ClanNotExistingInDB()
        {
            _repository.Setup(x => x.ClanFind(It.IsAny<int>())).Returns(null as Korisnik);

            var result = _controller.EditClan(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void EditClan_ClanExistingInDB()
        {
            Korisnik clan = new Korisnik
            {
                Ime = "Ime",
                Prezime = "Prezime"
            };
            _repository.Setup(x => x.ClanFind(It.IsAny<int>())).Returns(clan);

            var result = _controller.EditClan(1) as ViewResult;
            Assert.Equal("EditClan", result.ViewName);
            var model = result.Model as AdministracijaEditClanaVM;
            Assert.Equal(clan.Ime, model.Ime);
            Assert.Equal(clan.Prezime, model.Prezime);
        }
        [Fact]
        public void PrikazClanova_ReturnsView_WithModel()
        {
            _repository.Setup(x => x.BrojAktivnihClanarina()).Returns(25);
            _repository.Setup(x => x.GetTipoviClanarine()).Returns(new List<SelectListItem> { new SelectListItem(), new SelectListItem() });
            _helper.Setup(x => x.ClanarineAN()).Returns(new List<SelectListItem> { new SelectListItem() });

            var result = _controller.PrikazClanova() as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazClanovaVM>(result.Model);
            Assert.Equal("PrikazClanova", result.ViewName);
            Assert.Equal(25, model.BrojAktivnihClanova);
            Assert.Equal(1, model.ClanarineAN.Count);
        }
        [Fact]
        public void PrikazPrisutnihClanova()
        {
            var result = _controller.PrikazPrisutnihClanova() as ViewResult;
            Assert.Equal("PrikazPrisutnihClanova", result.ViewName);
        }
        [Fact]
        public void ObrisiClana_ClanExistingInDB()
        {
            _repository.Setup(r => r.ClanFind(It.IsAny<int>())).Returns(new Korisnik());

            var result = _controller.ObrisiKorisnika(1) as RedirectToActionResult;
            _repository.Verify(x => x.ClanUpdate(It.IsAny<Korisnik>()), Times.Once);
            Assert.Equal("PrikazClanova", result.ActionName);
        }
        [Fact]
        public void ObrisiClana_ClanNotExistingInDB()
        {
            _repository.Setup(x => x.ClanFind(It.IsAny<int>())).Returns(null as Korisnik);

            var result = _controller.ObrisiKorisnika(1) as ViewResult;
            _repository.Verify(x => x.ClanUpdate(It.IsAny<Korisnik>()), Times.Never);
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void DodajClanarinu_ClanNotExistingInDB()
        {
            _repository.Setup(x => x.ClanFind(It.IsAny<int>())).Returns(null as Korisnik);

            var result = _controller.DodajClanarinu(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void DodajClanarinu_ClanExistingInDB()
        {
            _repository.Setup(x => x.ClanFind(It.IsAny<int>())).Returns(new Korisnik { ID = 1 });
            var result = _controller.DodajClanarinu(1) as ViewResult;
            var model = Assert.IsType<AdministracijaDodajClanarinuVM>(result.Model);
            Assert.Equal(1, model.ClanID);
            Assert.Equal("DodajClanarinu", result.ViewName);
        }
        [Fact]
        public void SpremiClanarinu_BadModel()
        {
            _controller.ModelState.AddModelError("clanID", "clanId je null");
            _repository.Verify(x => x.DodajClanarinu(It.IsAny<Clanarina>()), Times.Never);
            var result = _controller.SpremiClanarinu(new AdministracijaDodajClanarinuVM()) as ViewResult;
            var model = Assert.IsType<AdministracijaDodajClanarinuVM>(result.Model);
            Assert.Equal("DodajClanarinu", result.ViewName);
        }
        [Fact]
        public void SpremiClanarinu_GoodModel()
        {
            var result = _controller.SpremiClanarinu(new AdministracijaDodajClanarinuVM()) as RedirectToActionResult;
            _repository.Verify(x => x.DodajClanarinu(It.IsAny<Clanarina>()), Times.Once);
            Assert.Equal("ClanDetalji", result.ActionName);
        }
        [Fact]
        public void PrikazClanarina()
        {
            _repository.Setup(x => x.GetTipoviClanarine()).Returns(new List<SelectListItem> { new SelectListItem(), new SelectListItem() });
            _repository.Setup(x => x.BrojAktivnihClanarina()).Returns(10);
            _helper.Setup(x => x.ClanarineAN()).Returns(new List<SelectListItem> { new SelectListItem(), new SelectListItem() });
            var result = _controller.PrikazClanarina() as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazClanarinaVMs>(result.Model);
            Assert.Equal(3, model.tipoviClanarine.Count);
            Assert.Equal(10, model.UkupnoAktivnih);
            Assert.Equal(2, model.statusi.Count);
        }
        [Fact]
        public void PrikazTipovaClanarina()
        {
            _repository.Setup(x => x.GetAllTipoviClanarine()).Returns(new List<TipClanarine> { new TipClanarine(), new TipClanarine() });
            var result = _controller.PrikazTipovaClanarina() as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazTipovaClanarinaVM>(result.Model);
            Assert.Equal(2, model.tipoviClanarine.Count);
            Assert.Equal("PrikazTipovaClanarina", result.ViewName);
        }
        [Fact]
        public void DodajTipClanarine()
        {
            var result = _controller.DodajTipClanarine() as ViewResult;
            Assert.IsType<AdministracijaDodajTipClanarineVM>(result.Model);
            Assert.Equal("DodajTipClanarine", result.ViewName);
        }
        [Fact]
        public void EditTipClanarine_NotExistingInDB()
        {
            _repository.Setup(x => x.TipClanarineFind(It.IsAny<int>())).Returns(null as TipClanarine);
            var result = _controller.EditTipClanarine(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void SnimiTipClanarine_BadModel()
        {
            AdministracijaDodajTipClanarineVM tipClanarineVM = new AdministracijaDodajTipClanarineVM
            {
                Cijena = 10,
                Trajanje = 20
            };
            _controller.ModelState.AddModelError("Naziv", "Naziv is required");
            var result = _controller.SnimiTipClanarine(tipClanarineVM) as ViewResult;
            var model = Assert.IsType<AdministracijaDodajTipClanarineVM>(result.Model);
            _repository.Verify(x => x.DodajTipClanarine(It.IsAny<TipClanarine>()), Times.Never);
            _repository.Verify(x => x.UpdateTipClanarine(It.IsAny<TipClanarine>()), Times.Never);
            Assert.Equal(tipClanarineVM.Cijena, model.Cijena);
            Assert.Equal(tipClanarineVM.Trajanje, model.Trajanje);
            Assert.Equal("DodajTipClanarine", result.ViewName);
        }
        [Fact]
        public void SnimitiTipClanarine_GoodModel_DoesntExistInDB()
        {
            _repository.Setup(x => x.TipClanarineFind(It.IsAny<int>())).Returns(new TipClanarine());
            var result = _controller.SnimiTipClanarine(new AdministracijaDodajTipClanarineVM()) as ViewResult;
            _repository.Verify(x => x.DodajTipClanarine(It.IsAny<TipClanarine>()), Times.Never);
            _repository.Verify(x => x.UpdateTipClanarine(It.IsAny<TipClanarine>()), Times.Once);
        }
        [Fact]
        public void ObrisiTipClanarine_DoesntExistInDB()
        {
            _repository.Setup(x => x.TipClanarineFind(It.IsAny<int>())).Returns(null as TipClanarine);
            var result = _controller.ObrisiTipClanarine(1) as ViewResult;
            _repository.Verify(x => x.UpdateTipClanarine(It.IsAny<TipClanarine>()), Times.Never);
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void ObrisiTipClanarine_ExistsInDB()
        {
            _repository.Setup(x => x.TipClanarineFind(It.IsAny<int>())).Returns(new TipClanarine());
            var result = _controller.ObrisiTipClanarine(1) as RedirectToActionResult;
            _repository.Verify(x => x.UpdateTipClanarine(It.IsAny<TipClanarine>()), Times.Once);
            Assert.Equal("PrikazTipovaClanarina", result.ActionName);
        }
        [Fact]
        public void ClanDetalji_DoesntExistInDB()
        {
            _repository.Setup(x => x.ClanFind(It.IsAny<int>())).Returns(null as Korisnik);
            var result = _controller.ClanDetalji(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void ClanDetalji_ExistsInDB()
        {
            Clanarina clanarina = new Clanarina
            {
                DatumDodavanja = new DateTime(2019, 1, 1),
                DatumIsteka = new DateTime(2019, 2, 2),
                TipClanarineID = 1
            };
            string prvaClanarina = "12.12.2018";
            _repository.Setup(x => x.ClanFind(It.IsAny<int>())).Returns(new Korisnik());
            _repository.Setup(x => x.AktivnaClanarina(It.IsAny<int>())).Returns(clanarina);
            _repository.Setup(x => x.DatumPrveClanarine(It.IsAny<int>())).Returns(prvaClanarina);
            var result = _controller.ClanDetalji(1) as ViewResult;
            var model = Assert.IsType<AdministracijaDetaljiClanaVM>(result.Model);
            Assert.Equal(model.clanarina.DatumDodavanja, clanarina.DatumDodavanja);
            Assert.Equal(model.clanarina.DatumIsteka, clanarina.DatumIsteka);
            Assert.Equal(model.clanarina.TipClanarineID, clanarina.TipClanarineID);
            Assert.Equal(prvaClanarina, model.DatumPrveClanarine);
            Assert.Equal("ClanDetalji", result.ViewName);
        }
    }
}
