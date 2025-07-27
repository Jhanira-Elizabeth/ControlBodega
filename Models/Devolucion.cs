using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlBodega.Models
{
    public class Devolucion
    {
        [Key]
        public int IdDevolucion { get; set; }

        [Required]
        [Display(Name = "ID Egreso")]
        public int IdEgreso { get; set; }

        [ForeignKey("IdEgreso")]
        public Egreso? Egreso { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public string Producto { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Planta")]
        public int IdPlanta { get; set; }

        [ForeignKey("IdPlanta")]
        public Planta? Planta { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required]
        [Display(Name = "Fecha de Devolución")]
        public DateTime FechaDevolucion { get; set; }

        [Display(Name = "Motivo")]
        public string? Motivo { get; set; }
    }
}
