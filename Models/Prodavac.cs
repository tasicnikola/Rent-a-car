using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Models
{
    public class Prodavac
    {
        [Key]
        public int ID { get; set; }

        [RegularExpression("\\d+")]
        [Required]
        [MaxLength(9)]
        public long BrLicneKarte { get; set; }

        [RegularExpression("\\w+")]
        [MaxLength(15)]
        [Required]
        public string Ime { get; set; }

        [RegularExpression("\\w+")]
        [MaxLength(15)]
        [Required]
        public string Prezime { get; set; }
        
        [RegularExpression("\\d+")]
        [Range(0,999999999)]
        public int Telefon { get; set; } 
        public string Adresa { get; set; }
        [JsonIgnore]                      
        public List<Vozilo> ListaVozila { get; set; }
    }
}