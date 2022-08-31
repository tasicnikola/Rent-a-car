using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections.Generic;
namespace Models
{
    public class TipKaroserije
    {
      [Key]
      public int ID { get; set; }

      [Required]
      public string Naziv { get; set; }

      public string Opis { get; set; }
      
      [JsonIgnore]
      public List<Vozilo> VozilaSaTipomKaroserije { get; set; }
    }
    
}