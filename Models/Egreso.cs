using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace ControlBodega.Models
{
    public class Egreso
    {
        [Key]
        public int IdEgreso { get; set; }
        [Required]
        public int IdPlanta { get; set; }
        [ForeignKey("IdPlanta")]
        public Planta? Planta { get; set; }
        public DateTime Fecha { get; set; }
        [Required, StringLength(100)]
        public string Producto { get; set; } = string.Empty;
        [Required, StringLength(50)]
        public string Medida { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        [StringLength(250)]
        public string? Responsable { get; set; }
        [StringLength(250)]
        public string? Tipo { get; set; }
    }
}
