using FitnessCentar.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessCentar.service.Interfaces
{
    public interface IPlanIProgramService
    {
        IEnumerable<PlanIProgram> getPlanovi();
        IEnumerable<KategorijaPlanIProgram> getKategorije();
        void DodajPlanIProgram(PlanIProgram planIProgram);
        void DodajSedmicu(Sedmica sedmica);
        void DodajDan(Dan dan);
        PlanIProgram PlanIProgramFind(int id);
        IEnumerable<Sedmica> GetSedmiceOrderdByRedniBroj(int planID);
        IEnumerable<Dan> GetDaniOrderedByRedniBroj(int sedmicaID);
        void UpdatePlanIProgram(PlanIProgram planIProgram);
        Sedmica SedmicaFind(int id);
        IEnumerable<DanVjezba> GetDanVjezbe(int sedmicaID);
        IEnumerable<Dan> GetDan(int sedmicaID);
        void ObrisiDanVjezba(DanVjezba danVjezba);
        void ObrisiSedmicu(Sedmica sedmica);
        void ObrisiDan(Dan dan);
        Vjezba VjezbaFind(int id);
        void DodajVjezbu(Vjezba vjezba);
        void UpdateVjezbu(Vjezba vjezba);
        IEnumerable<Vjezba> GetVjezbe();
        IEnumerable<KategorijaPlanIProgram> GetKategorije();
        void DodajKategoriju(KategorijaPlanIProgram kategorija);
        KategorijaPlanIProgram KategorijaPlanIProgramFind(int id);
        void UpdateKategoriju(KategorijaPlanIProgram kategorija);
        void ObrisiVjezbu(Vjezba vjezba);
    }
}
