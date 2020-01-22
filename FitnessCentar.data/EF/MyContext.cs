using FitnessCentar.data.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCentar.data.EF
{
    public class MyContext : DbContext
    {
        public MyContext() { }
        public MyContext(DbContextOptions<MyContext> x) : base(x)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Korisnik>().HasIndex(k => new {k.Email,k.BrojKartice,k.JMBG }).IsUnique(true);
            builder.Entity<Narudzba>().HasOne(x => x.Korisnik).WithMany().OnDelete(DeleteBehavior.Restrict);
            builder.Entity<NarudzbaStavke>().HasOne(x => x.Velicina).WithMany().OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Adresa> Adresa { get; set; }
        public DbSet<Clanarina> Clanarina { get; set; }
        public DbSet<Kategorija> Kategorija { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<MjerenjeNapredka> MjerenjeNapredka { get; set; }
        public DbSet<NacinPlacanja> NacinPlacanja { get; set; }
        public DbSet<Narudzba> Narudzba { get; set; }
        public DbSet<NarudzbaStavke> NarudzbaStavke { get; set; }
        public DbSet<Podkategorija> Podkategorija { get; set; }
        public DbSet<PosjecenostClana> PosjecenostClana { get; set; }
        public DbSet<Racun> Racun { get; set; }
        public DbSet<Skladiste> Skladiste { get; set; }
        public DbSet<Stavka> Stavka { get; set; }
        public DbSet<TipClanarine> TipClanarine { get; set; }
        public DbSet<Vjezba> Vjezba { get; set; }
        public DbSet<PlanIProgram> PlanIProgram { get; set; }
        public DbSet<Dan> Dan { get; set; }
        public DbSet<DanVjezba> DanVjezba { get; set; }
        public DbSet<Sedmica> Sedmica { get; set; }
        public DbSet<KategorijaPlanIProgram> KategorijaPlanIProgram { get; set; }
        public DbSet<KorisnickiNalog> KorisnickiNalog { get; set; }
        public DbSet<AutorizacijskiToken> AutorizacijskiToken { get; set; }

    }
}
