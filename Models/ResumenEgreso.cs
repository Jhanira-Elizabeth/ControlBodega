using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace ControlBodega.Models
{
    public class ResumenEgreso
    {
        [Key]
        public int IdResumenEgreso { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Total Egresos")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalEgresos { get; set; }

        [Display(Name = "Planta")]
        public int IdPlanta { get; set; }
        [ForeignKey("IdPlanta")]
        public Planta? Planta { get; set; }
    }
}
