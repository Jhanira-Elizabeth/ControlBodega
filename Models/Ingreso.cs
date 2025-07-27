using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace ControlBodega.Models
{
    public class Ingreso
    {
        [Key]
        public int IdIngreso { get; set; }
        [Required]
        public int IdPlanta { get; set; }
        [ForeignKey("IdPlanta")]
        public Planta? Planta { get; set; }
        [Required, StringLength(100)]
        public string Producto { get; set; } = string.Empty;
        [Required, StringLength(50)]
        public string Medida { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
