using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Miembro
{
    [Key]
    public int MiembroID { get; set; }

    [Required]
    [ForeignKey("Suscripcion")]
    public int SubscriptionID { get; set; }

    [Required]
    [StringLength(100)]
    public string NombreMiembro { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? EmailOpcional { get; set; }

    [Required]
    [StringLength(20)]
    public string Rol { get; set; }

    [Range(0, 9999.99)]
    public decimal? MontoAportado { get; set; }

    public string? ApplicationUserId { get; set; } 

   
    public Suscripcion? Suscripcion { get; set; }
}
