using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Plan
{
    [Key]
    public int PlanID { get; set; }

    [Required]
    [ForeignKey("Plataforma")]
    public int PlataformaID { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    [Range(0, 9999.99)]
    public decimal Precio { get; set; }

    [Required]
    [StringLength(20)]
    public string Periodicidad { get; set; } 

    [StringLength(255)]
    public string Descripcion { get; set; }

    public Plataforma Plataforma { get; set; }
    public ICollection<Suscripcion> Suscripciones { get; set; } = new List<Suscripcion>();
}
