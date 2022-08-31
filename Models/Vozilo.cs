using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Vozilo")]
    public class Vozilo
    {
        [Key]
        public int ID { get; set; }

        [RegularExpression("\\w+")] //slova samo
        [Required]
        [MaxLength(15)]
        public string Marka { get; set; }

        [MaxLength(15)]
        public string Model { get; set; }

        [Required]
        [MaxLength(8)]
        public string RegistarskaTablica { get; set; }

        [RegularExpression("\\d+")] //samo brojevi
        [Range(100,200000)]
        public int Cena { get; set; }

        [RegularExpression("\\d+")] //samo brojevi
        [Range(1960,2021)]
        public int GodinaProizvodnje { get; set; }

        [RegularExpression("\\d+")]
        public int Kilometraza { get; set; }

        [RegularExpression("\\d+")]
        [Range(20,8000)]
        public int ZapreminaMotora { get; set; }

        [RegularExpression("\\d+")]
        [Range(20,1000)]
        public int SnagaMotora { get; set; }

        [RegularExpression("\\w+")] 
        [MaxLength(15)]
        public TipKaroserije Karoserija { get; set; }

        public AutoPlac NazivPlaca { get; set; }

        public Prodavac Vlasnik { get; set;}
    }
}