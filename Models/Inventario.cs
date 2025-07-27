using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ControlBodega.Models
{
    public class Inventario
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string? Producto { get; set; }
        [Required, StringLength(50)]
        public string? Medida { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public decimal Precio { get; set; }
        [Required]
        public int IdPlanta { get; set; }
        [ForeignKey("IdPlanta")]
        public Planta? Planta { get; set; }
    }
}
