using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using FitnessCentar.web.Controllers;
using FitnessCentar.web.ViewModels.AdministracijaVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace FitnessCentar.test.AdministracijaTests
{
    public class AdministracijaPlanIProgramUnitTests
    {
        private Mock<IPlanIProgramService> _service;
        private Mock<IHostingEnvironment> _hosting;
        private AdministracijaPlanIProgramController _controller;
        public AdministracijaPlanIProgramUnitTests()
        {
            _service = new Mock<IPlanIProgramService>();
            _hosting = new Mock<IHostingEnvironment>();
            _controller = new AdministracijaPlanIProgramController(_service.Object,_hosting.Object);
        }
        [Fact]
        public void PlanIProgramLista_ReturnsViewWithModel()
        {
            _service.Setup(x => x.getPlanovi()).Returns(new List<PlanIProgram> { new PlanIProgram { Kategorija = new KategorijaPlanIProgram() }, new PlanIProgram { Kategorija = new KategorijaPlanIProgram() }, new PlanIProgram { Kategorija = new KategorijaPlanIProgram() } });
            var result = _controller.PlanIProgramLista() as ViewResult;
            var model = Assert.IsType<AdministracijaPlanIProgramVM>(result.Model);
            Assert.Equal(3, model.Planovi.Count);
            Assert.Equal("PlanIProgramLista", result.ViewName);
        }
        [Fact]
        public void DodajPlanIProgram_ReturnsViewWithModel()
        {
            _service.Setup(x => x.getKategorije()).Returns(new List<KategorijaPlanIProgram> { new KategorijaPlanIProgram(), new KategorijaPlanIProgram() });
            var result = _controller.DodajPlanIProgram() as ViewResult;
            var model = Assert.IsType<AdministracijaDodajPlanIProgramVM>(result.Model);
            Assert.Equal(2, model.Kategorije.Count);
            Assert.Equal("DodajPlanIProgram", result.ViewName);
        }
        [Fact]
        public void SpremiPlanIProgram_BadModel()
        {
            _service.Setup(x => x.getKategorije()).Returns(new List<KategorijaPlanIProgram> { new KategorijaPlanIProgram() });
            _controller.ModelState.AddModelError("Naziv", "Naziv is required!");
            var result = _controller.SpremiPlanIProgram(new AdministracijaDodajPlanIProgramVM()) as ViewResult;
            var model = Assert.IsType<AdministracijaDodajPlanIProgramVM>(result.Model);
            Assert.Equal(1, model.Kategorije.Count);
            Assert.Equal("DodajPlanIProgram", result.ViewName);
            _service.Verify(x => x.DodajPlanIProgram(It.IsAny<PlanIProgram>()), Times.Never);
            _service.Verify(x => x.DodajSedmicu(It.IsAny<Sedmica>()), Times.Never);
            _service.Verify(x => x.DodajDan(It.IsAny<Dan>()), Times.Never);
        }
        [Fact]
        public void SpremiPlanIProgram_GoodModel()
        {
            AdministracijaDodajPlanIProgramVM planIProgramVM = new AdministracijaDodajPlanIProgramVM
            {
                BrojSedmica = 3
            };
            var result = _controller.SpremiPlanIProgram(planIProgramVM) as RedirectToActionResult;
            _service.Verify(x => x.DodajPlanIProgram(It.IsAny<PlanIProgram>()), Times.Once);
            _service.Verify(x => x.DodajSedmicu(It.IsAny<Sedmica>()), Times.Exactly(planIProgramVM.BrojSedmica));
            _service.Verify(x => x.DodajDan(It.IsAny<Dan>()), Times.Exactly(planIProgramVM.BrojSedmica * 7));
            Assert.Equal("PrikazPlanIProgram", result.ActionName);
        }
        [Fact]
        public void PrikazPlanIProgram_NotExistsInDB()
        {
            _service.Setup(x => x.PlanIProgramFind(It.IsAny<int>())).Returns(null as PlanIProgram);
            var result = _controller.PrikazPlanIProgram(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void PrikazPlanIProgram_ExistsInDB()
        {
            _service.Setup(x => x.PlanIProgramFind(It.IsAny<int>())).Returns(new PlanIProgram { Kategorija = new KategorijaPlanIProgram()});
            List<Sedmica> sedmice = new List<Sedmica> { new Sedmica { }, new Sedmica { } };
            List<Dan> dani = new List<Dan> { new Dan(), new Dan(), new Dan(), new Dan(), new Dan(), new Dan(), new Dan() };
            _service.Setup(x => x.GetSedmiceOrderdByRedniBroj(It.IsAny<int>())).Returns(sedmice);
            _service.Setup(x => x.GetDaniOrderedByRedniBroj(It.IsAny<int>())).Returns(dani);
            var result = _controller.PrikazPlanIProgram(1) as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazPlanIProgramVM>(result.Model);
            Assert.Equal("PrikazPlanIProgram", result.ViewName);
            Assert.Equal(sedmice.Count, model.Sedmice.Count);
            foreach(var x in model.Sedmice)
            {
                Assert.Equal(dani.Count, x.Dani.Count);
            }
        }
        [Fact]
        public void ObrisiPlanIProgram_NotExistsInDB()
        {
            _service.Setup(x => x.PlanIProgramFind(It.IsAny<int>())).Returns(null as PlanIProgram);
            var result = _controller.ObrisiPlanIProgram(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
            _service.Verify(x => x.UpdatePlanIProgram(It.IsAny<PlanIProgram>()), Times.Never);
        }
        [Fact]
        public void ObrisiPlanIProgram_ExistsInDB()
        {
            _service.Setup(x => x.PlanIProgramFind(It.IsAny<int>())).Returns(new PlanIProgram());
            var result = _controller.ObrisiPlanIProgram(1) as RedirectToActionResult;
            Assert.Equal("PlanIProgramLista", result.ActionName);
            _service.Verify(x => x.UpdatePlanIProgram(It.IsAny<PlanIProgram>()), Times.Once);
        }
        [Fact]
        public void ObrisiSedmicu_NotExistsInDB()
        {
            _service.Setup(x => x.SedmicaFind(It.IsAny<int>())).Returns(null as Sedmica);
            var result = _controller.ObrisiSedmicu(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
            _service.Verify(x => x.ObrisiSedmicu(It.IsAny<Sedmica>()), Times.Never);
            _service.Verify(x => x.ObrisiDanVjezba(It.IsAny<DanVjezba>()), Times.Never);
            _service.Verify(x => x.ObrisiDan(It.IsAny<Dan>()), Times.Never);
        }
        [Fact]
        public void ObrisiSedmicu_ExistsInDB()
        {
            List<DanVjezba> danVjezbe = new List<DanVjezba> { new DanVjezba(), new DanVjezba(), new DanVjezba() };
            List<Dan> dani = new List<Dan> { new Dan(), new Dan(), new Dan(), new Dan() };
            _service.Setup(x => x.GetDan(It.IsAny<int>())).Returns(dani);
            _service.Setup(x => x.GetDanVjezbe(It.IsAny<int>())).Returns(danVjezbe);
            _service.Setup(x => x.SedmicaFind(It.IsAny<int>())).Returns(new Sedmica());
            var result = _controller.ObrisiSedmicu(1) as RedirectToActionResult;
            Assert.Equal("PrikazPlanIProgram", result.ActionName);
            _service.Verify(x => x.ObrisiSedmicu(It.IsAny<Sedmica>()), Times.Once);
            _service.Verify(x => x.ObrisiDanVjezba(It.IsAny<DanVjezba>()), Times.Exactly(danVjezbe.Count));
            _service.Verify(x => x.ObrisiDan(It.IsAny<Dan>()), Times.Exactly(dani.Count));
            
        }
        [Fact]
        public void DodajVjezbu_ReturnsViewWithModel()
        {
            var result = _controller.DodajVjezbu() as ViewResult;
            Assert.IsType<AdministracijaDodajVjezbuVM>(result.Model);
            Assert.Equal("DodajVjezbu", result.ViewName);
        }
        [Fact]
        public void EditVjezbu_NotExistsInDB()
        {
            _service.Setup(x => x.VjezbaFind(It.IsAny<int>())).Returns(null as Vjezba);
            var result = _controller.EditVjezbu(1) as ViewResult;
            Assert.Equal("NotFound", result.ViewName);
        }
        [Fact]
        public void EditVjezbu_ExistsInDB()
        {
            Vjezba vjezba = new Vjezba
            {
                ID = 1,
                Naziv = "Naziv",
                Opis = "Opis"
            };
            _service.Setup(x => x.VjezbaFind(It.IsAny<int>())).Returns(vjezba);
            var result = _controller.EditVjezbu(1) as ViewResult;
            var model = Assert.IsType<AdministracijaEditVjezbuVM>(result.Model);
            Assert.Equal(vjezba.ID, model.ID);
            Assert.Equal(vjezba.Naziv, model.Naziv);
            Assert.Equal(vjezba.Opis, model.Opis);
            Assert.Equal("DodajVjezbu", result.ViewName);
        }
        [Fact]
        public void SpremiVjezbu_BadModel()
        {
            AdministracijaDodajVjezbuVM vjezba = new AdministracijaDodajVjezbuVM
            {
                Opis = "ASDAS"
            };
            _controller.ModelState.AddModelError("Naziv", "Naziv is required!");
            var result = _controller.SpremiVjezbu(vjezba) as ViewResult;
            var model = Assert.IsType<AdministracijaDodajVjezbuVM>(result.Model);
            Assert.Equal(vjezba.Opis, model.Opis);
            Assert.Equal("DodajVjezbu", result.ViewName);
            _service.Verify(x => x.DodajVjezbu(It.IsAny<Vjezba>()), Times.Never);
            _service.Verify(x => x.UpdateVjezbu(It.IsAny<Vjezba>()), Times.Never);
        }
        [Fact]
        public void SpremiVjezbu_GoodModel_ExistingVjezba()
        {
            _service.Setup(x => x.VjezbaFind(It.IsAny<int>())).Returns(new Vjezba());
            var result = _controller.SpremiVjezbu(new AdministracijaDodajVjezbuVM()) as RedirectToActionResult;
            _service.Verify(x => x.DodajVjezbu(It.IsAny<Vjezba>()), Times.Never);
            _service.Verify(x => x.UpdateVjezbu(It.IsAny<Vjezba>()), Times.Once);
            Assert.Equal("PrikazVjezbi", result.ActionName);
        }
        [Fact]
        public void SpremiVjezbu_GoodModel_NewVjezba()
        {
            _service.Setup(x => x.VjezbaFind(It.IsAny<int>())).Returns(null as Vjezba);
            var result = _controller.SpremiVjezbu(new AdministracijaDodajVjezbuVM()) as RedirectToActionResult;
            _service.Verify(x => x.DodajVjezbu(It.IsAny<Vjezba>()), Times.Once);
            _service.Verify(x => x.UpdateVjezbu(It.IsAny<Vjezba>()), Times.Once);
            Assert.Equal("PrikazVjezbi", result.ActionName);
        }
        [Fact]
        public void PrikazVjezbi_ReturnsViewWithModel()
        {
            List<Vjezba> vjezbe = new List<Vjezba> { new Vjezba(), new Vjezba() };
            _service.Setup(x => x.GetVjezbe()).Returns(vjezbe);
            var result = _controller.PrikazVjezbi() as ViewResult;
            var model = Assert.IsType<AdministracijaPrikazVjezbiVM>(result.Model);
            Assert.Equal(2, model.Vjezbe.Count);
            Assert.Equal("PrikazVjezbi", result.ViewName);
        }
    }
}
