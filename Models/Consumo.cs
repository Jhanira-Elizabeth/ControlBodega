using System;
using System.ComponentModel.DataAnnotations;

namespace ControlBodega.Models
{
    public class Consumo
    {
        public int IdPlanta { get; set; }
        public Planta? Planta { get; set; }
        public int Cantidad { get; set; }
        [Key]
        public int IdConsumo { get; set; }
        public int IdEgreso { get; set; }
        public Egreso? Egreso { get; set; }
        [Required, StringLength(100)]
        public string Producto { get; set; } = string.Empty;
        [Required, StringLength(50)]
        public string Medida { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Hora { get; set; } = string.Empty;
        [StringLength(100)]
        public string Tanque { get; set; } = string.Empty;
        [StringLength(100)]
        public string Poblacion { get; set; } = string.Empty;
        [StringLength(100)]
        public string Estadio { get; set; } = string.Empty;
    }
}
