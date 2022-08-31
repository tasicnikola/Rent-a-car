using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    public class AutoPlac
    {
        [Key]
        public int ID { get; set; }

        [RegularExpression("\\w+")]
        [Required]
        [MaxLength(15)]
        public string Naziv { get; set; }

        [RegularExpression("\\d+")]
        [Range(0,999999999)]
        public int Telefon { get; set; } 
        public string Adresa { get; set; }
        
        [Range (1,100)]
        public int Kapacitet { get; set; }

        [JsonIgnore]    
        public List<Vozilo> Vozila{ get; set; }
    }
}