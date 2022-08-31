using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
namespace WEBPROJEKAT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoziloController : ControllerBase
    {
        public AutoPlacContext Context { get; set; } 

        public VoziloController(AutoPlacContext context)
        {
            Context=context;
        }
        [Route("PrikazPoObKaroserije/{idKaroserije}/{naziv}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPoOblikuKaroserije(int idKaroserije, string naziv)
        {
            var auta=Context.Vozila
                            .Include(p=>p.Vlasnik).Where(p=>p.Karoserija.ID==idKaroserije && p.NazivPlaca.Naziv==naziv);
            var auto=await auta.ToListAsync();
            return Ok
            (
                auto.Select(p=>
                new
                {
                    ID=p.ID,
                    Marka=p.Marka,
                    Model=p.Model,
                    GodinaProizvodnje=p.GodinaProizvodnje,
                    ImeVlasnika=p.Vlasnik.Ime,
                    BrojTelefona=p.Vlasnik.Telefon,
                    Tablica=p.RegistarskaTablica,
                    Cena=p.Cena,

                })); 
        }

        [Route("Prikaz/{id}")]
        [HttpGet]
        public async Task<ActionResult> Preuzmi(int id)
        {
            var auta=Context.Vozila
                            .Include(p=>p.Vlasnik).Where(p=>p.ID==id)
                            .Include(p=>p.Karoserija)
                            .Include(p=>p.NazivPlaca);
            var auto=await auta.ToListAsync();
            return Ok(auto); 
        }

        [Route("DodajVozilo/{Marka}/{Model}/{Registarska_tablica}/{Cena}/{God_Proizvodnje}/{Kilometraza}/{Zapremina_motora}/{Snaga_motora_u_ks}/{Karoserija}/{Plac_naziv}/{Vlasnik_brLicneKarte}")]
        [HttpPost]
        public async Task<ActionResult> DodajVozilo(string Marka, string Model, string Registarska_tablica, int Cena, int God_Proizvodnje, int Kilometraza, int Zapremina_motora, int Snaga_motora_u_ks, string Karoserija, string Plac_naziv, long Vlasnik_brLicneKarte) //da moze da vrati json, ok ili bad request
        {
            if(God_Proizvodnje<1960 || God_Proizvodnje>2021)
            {
                return BadRequest("Godina van preporucenog opsega (1960-2021)!");
            }
            if(string.IsNullOrWhiteSpace(Marka) || Marka.Length>15)
            {
                return BadRequest("Pogresna ili nevalidna marka automobila!");
            }
            if(Model.Length>15)
            {
                return BadRequest("Predug naziv modela!");
            }
            if(string.IsNullOrWhiteSpace(Karoserija))
            {
                return BadRequest("Unesite tip karoserije!");
            }
            if(Snaga_motora_u_ks>1000 || Snaga_motora_u_ks<20)
            {
                return BadRequest("Snaga motora van opsega! Preporucen opseg 20-1000ks");
            }
            if(Zapremina_motora>8000 || Zapremina_motora<20)
            {
                return BadRequest("Zapremina motora van preporucenog opsega!");
            }
            try
            {
                var vozZaProveru=await Context.Vozila.Where(p=>p.RegistarskaTablica==Registarska_tablica).FirstOrDefaultAsync();
                if(vozZaProveru!=null){
                    return StatusCode(203,"Vozilo sa datom registarskom tablicom vec postoji u bazi!");
                }
                var vlasnik=await Context.Prodavci.Where(p=>p.BrLicneKarte==Vlasnik_brLicneKarte).FirstOrDefaultAsync();
                if(vlasnik==null)
                {
                    return StatusCode(204,"Proverite da li je broj licne karte ispravan ili prvo unesite vlasnika sa datim brojem!");
                }
                var plac=await Context.Placevi.Where(p=>p.Naziv==Plac_naziv).FirstOrDefaultAsync();
                var karoserija= await Context.Karoserije.Where(p=>p.Naziv==Karoserija).FirstOrDefaultAsync();
                Vozilo novoVozilo=new Vozilo();
                novoVozilo.Marka=Marka;
                novoVozilo.Model=Model;
                novoVozilo.RegistarskaTablica=Registarska_tablica;
                novoVozilo.Cena=Cena;
                novoVozilo.GodinaProizvodnje=God_Proizvodnje;
                novoVozilo.Kilometraza=Kilometraza;
                novoVozilo.ZapreminaMotora=Zapremina_motora;
                novoVozilo.SnagaMotora=Snaga_motora_u_ks;
                novoVozilo.Marka=Marka;
                novoVozilo.Karoserija=karoserija;
                novoVozilo.Vlasnik=vlasnik;
                novoVozilo.NazivPlaca=plac;
                Context.Vozila.Add(novoVozilo);
                await Context.SaveChangesAsync();

                var sviAutomobiliVlasnika=await Context.Vozila
                                          .Include(p=>p.Vlasnik).Where(p=>p.Vlasnik==vlasnik && p.NazivPlaca==plac)
                                          .Select(p=>
                                          new
                                          {
                                              ID=p.ID,
                                              Marka=p.Marka,
                                              Model=p.Model,
                                              GodinaProizvodnje=p.GodinaProizvodnje,
                                              ImeVlasnika=p.Vlasnik.Ime,
                                              BrojTelefona=p.Vlasnik.Telefon,
                                              Tablice=p.RegistarskaTablica,
                                              Cena=p.Cena,
                                          }
                                          ).ToListAsync();
                return Ok(sviAutomobiliVlasnika);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        [Route("PromeniCenu/{cena}/{id}")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int cena,int id)
        {
            if(cena<100 || cena>200000)
            {
                return StatusCode(206,"Neispravna cena!");
            }
            try
            {
                var voziloZaPromenu=await Context.Vozila.FindAsync(id);
                if(voziloZaPromenu==null)
                {
                    return StatusCode(207,"Vozilo je obrisano");
                }
                voziloZaPromenu.Cena=cena;
                await Context.SaveChangesAsync();
                
                return Ok("Uspesno promenjen!");

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("IzbrisiVozilo/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiVozilo(int id)
        {
            if(id<=0)
            {
                return BadRequest("Pogresan ID");
            }
            try
            {
                var vozilo=await Context.Vozila.FindAsync(id);
                if(vozilo==null)
                {
                    return StatusCode(208,"Vozilo ne postoji ili je izbrisano!");
                }
                Context.Vozila.Remove(vozilo);
                await Context.SaveChangesAsync();
                return Ok($"Vozilo marke {vozilo.Marka}, model {vozilo.Model} iz {vozilo.GodinaProizvodnje}. je uspesno izbrisano");

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }

}