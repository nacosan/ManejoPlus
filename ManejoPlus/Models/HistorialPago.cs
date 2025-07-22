using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HistorialPago
{
    [Key]
    public int PagoID { get; set; }

    [Required]
    [ForeignKey("Suscripcion")]
    public int SubscriptionID { get; set; }

    [Required]
    public DateTime FechaPago { get; set; }

    [Required]
    [Range(0.01, 9999.99)]
    public decimal Monto { get; set; }

    [StringLength(255)]
    public string Detalle { get; set; }

    public Suscripcion? Suscripcion { get; set; }
}
