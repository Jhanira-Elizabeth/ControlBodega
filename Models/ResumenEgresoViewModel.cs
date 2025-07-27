using System;
using System.Collections.Generic;

namespace ControlBodega.Models
{
    public class ResumenEgresoViewModel
    {
        public int? IdPlanta { get; set; }
        public string? NombrePlanta { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<Egreso> Egresos { get; set; } = new();
        public List<EgresoAlimentacion> EgresosAlimentacion { get; set; } = new();
        public List<Consumo> Consumos { get; set; } = new();
        public decimal TotalEgresos => Egresos.Sum(e => e.Cantidad);
        public decimal TotalEgresosAlimentacion => EgresosAlimentacion.Sum(e => e.Cantidad);
        public decimal TotalConsumos => Consumos.Sum(c => c.Cantidad);
    }

    public class ResumenEgresoItem
    {
        public DateTime Fecha { get; set; }
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
