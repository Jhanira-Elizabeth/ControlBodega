using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ControlBodega.Models
{
    public class Planta
    {
        [Key]
        public int IdPlanta { get; set; }
        [Required, StringLength(50)]
        public string NPlanta { get; set; } = string.Empty;
        public ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
    }
}