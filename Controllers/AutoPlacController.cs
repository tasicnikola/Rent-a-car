using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
namespace WEBPROJEKAT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class AutoPlacController: ControllerBase
    {
        public AutoPlacContext Context { get; set; }

        public AutoPlacController(AutoPlacContext context)
        {
            Context=context;
        }

        [Route("PreuzmiAutoPlaceve")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiAutoPlaceve()
        {
            return Ok(await Context.Placevi.Select(p=>new {p.ID, p.Naziv, p.Telefon, p.Adresa, p.Kapacitet}).ToListAsync());
        }

    }
}