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

    public class TipKaroserijeController: ControllerBase
    {
        public AutoPlacContext Context { get; set; }

        public TipKaroserijeController(AutoPlacContext context)
        {
            Context=context;
        }

        [Route("PreuzmiTipKaroserije")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiTipKaroserije()
        {
            return Ok(await Context.Karoserije.Select(p=>new {p.ID, p.Naziv, p.Opis}).ToListAsync());
        }
    }
}