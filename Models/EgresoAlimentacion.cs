using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace ControlBodega.Models
{
    public class EgresoAlimentacion
    {
        public int Cantidad { get; set; }
        [Key]
        public int IdEgresoA { get; set; }
        [Required]
        public int IdPlanta { get; set; }
        [ForeignKey("IdPlanta")]
        public Planta? Planta { get; set; }
        public DateTime Fecha { get; set; }
        [StringLength(100)]
        public string Responsable { get; set; } = string.Empty;
        [StringLength(100)]
        public string Producto { get; set; } = string.Empty;
    }
}
