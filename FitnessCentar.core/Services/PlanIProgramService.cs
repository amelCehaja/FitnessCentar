using FitnessCentar.data;
using FitnessCentar.data.Models;
using FitnessCentar.service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitnessCentar.service.Services
{
    public class PlanIProgramService : IPlanIProgramService 
    {
        IRepository<Vjezba> vjezbaRepository;
        IRepository<KategorijaPlanIProgram> kategorijaRepository;
        IRepository<PlanIProgram> planIProgramRepository;
        IRepository<Sedmica> sedmicaRepository;
        IRepository<Dan> danRepository;
        IRepository<DanVjezba> danVjezbaRepository;
        public PlanIProgramService(IRepository<Vjezba> _vjezbaRepository, IRepository<KategorijaPlanIProgram> _kategorijaRepository, IRepository<PlanIProgram> _planIProgramRepository, IRepository<Sedmica> _sedmicaRepository, IRepository<Dan> _danRepository, IRepository<DanVjezba> _danVjezbaRepository)
        {
            vjezbaRepository = _vjezbaRepository;
            kategorijaRepository = _kategorijaRepository;
            planIProgramRepository = _planIProgramRepository;
            sedmicaRepository = _sedmicaRepository;
            danRepository = _danRepository;
            danVjezbaRepository = _danVjezbaRepository;
        }
        public IEnumerable<PlanIProgram> getPlanovi()
        {
            return planIProgramRepository.GetPlanIProgram();
        }
        public IEnumerable<KategorijaPlanIProgram> getKategorije()
        {
            return planIProgramRepository.GetKategorijaPlanIProgram();
        }
        public void DodajPlanIProgram(PlanIProgram planIProgram)
        {
            planIProgramRepository.Add(planIProgram);
        }
        public void DodajSedmicu(Sedmica sedmica)
        {
            sedmicaRepository.Add(sedmica);
        }
        public void DodajDan(Dan dan)
        {
            danRepository.Add(dan);
        }
        public PlanIProgram PlanIProgramFind(int id)
        {
            return planIProgramRepository.GetPlanIProgram().Where(x => x.ID == id).SingleOrDefault();
        }
        public IEnumerable<Sedmica> GetSedmiceOrderdByRedniBroj(int planID)
        {
            return sedmicaRepository.GetSedmica().Where(x => x.PlanIProgramID == planID).OrderBy(x => x.RedniBroj);
        }
        public IEnumerable<Dan> GetDaniOrderedByRedniBroj(int sedmicaID)
        {
            return danRepository.GetDan().Where(x => x.SedmicaID == sedmicaID).OrderBy(x => x.RedniBroj);
        }
        public void UpdatePlanIProgram(PlanIProgram planIProgram)
        {
            planIProgramRepository.Update(planIProgram);
        }
        public Sedmica SedmicaFind(int id)
        {
            return sedmicaRepository.GetSedmica().Where(x => x.ID == id).SingleOrDefault();
        }
        public IEnumerable<DanVjezba> GetDanVjezbe(int sedmicaID)
        {
            return sedmicaRepository.GetDanVjezba().Where(x => x.Dan.SedmicaID == sedmicaID);
        }
        public IEnumerable<Dan> GetDan(int sedmicaID)
        {
            return sedmicaRepository.GetDan().Where(x => x.SedmicaID == sedmicaID);
        }
        public void ObrisiDanVjezba(DanVjezba danVjezba)
        {
            danVjezbaRepository.Remove(danVjezba);
        }
        public void ObrisiSedmicu(Sedmica sedmica)
        {
            sedmicaRepository.Remove(sedmica);
        }
        public void ObrisiDan(Dan dan)
        {
            danRepository.Remove(dan);
        }
        public Vjezba VjezbaFind(int id)
        {
            return vjezbaRepository.GetVjezbas().Where(x => x.ID == id).SingleOrDefault();
        }
        public void DodajVjezbu(Vjezba vjezba)
        {
            vjezbaRepository.Add(vjezba);
        }
        public void UpdateVjezbu(Vjezba vjezba)
        {
            vjezbaRepository.Update(vjezba);
        }
        public IEnumerable<Vjezba> GetVjezbe()
        {
            return vjezbaRepository.GetVjezbas();
        }
        public IEnumerable<KategorijaPlanIProgram> GetKategorije()
        {
            return planIProgramRepository.GetKategorijaPlanIProgram();
        }
        public void DodajKategoriju(KategorijaPlanIProgram kategorija)
        { 
            kategorijaRepository.Add(kategorija);
        }
        public KategorijaPlanIProgram KategorijaPlanIProgramFind(int id)
        {
            return kategorijaRepository.GetKategorijaPlanIProgram().Where(x => x.ID == id).SingleOrDefault();
        }
        public void UpdateKategoriju(KategorijaPlanIProgram kategorija)
        {
            kategorijaRepository.Update(kategorija);
        }
        public void ObrisiVjezbu(Vjezba vjezba)
        {
            vjezbaRepository.Remove(vjezba);
        }
    }
}
