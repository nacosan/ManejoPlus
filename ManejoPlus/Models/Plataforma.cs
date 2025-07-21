using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManejoPlus.Models;
[Table("Plataformas")]
public class Plataforma

{
   
    [Key]
    public int PlataformaID { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [StringLength(255)]
    public string Descripcion { get; set; }

    public bool EsPersonalizada { get; set; }

    public ICollection<Plan> Planes { get; set; } = new List<Plan>();
    public ICollection<Suscripcion> Suscripciones { get; set; } = new List<Suscripcion>();
}
