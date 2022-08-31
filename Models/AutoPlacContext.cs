using Microsoft.EntityFrameworkCore;
namespace Models
{
    public class AutoPlacContext : DbContext
    {   
        public DbSet<AutoPlac> Placevi { get; set; }
        public DbSet<Vozilo> Vozila { get; set; }
        public DbSet<Prodavac> Prodavci { get; set; }
        public DbSet<TipKaroserije> Karoserije { get; set; }

        public AutoPlacContext(DbContextOptions options):base(options)
        {

        }
    }
}