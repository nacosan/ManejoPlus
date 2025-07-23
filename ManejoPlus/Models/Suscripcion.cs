using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ManejoPlus.Models;

public class Suscripcion
{
    [Key]
    public int SubscriptionID { get; set; }

    [Required]
    [ForeignKey("Plataforma")]
    public int PlataformaID { get; set; }

    [Required]
    [ForeignKey("Plan")]
    public int PlanID { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    [StringLength(100)]
    public string NombrePersonalizado { get; set; }

    [Required]
    [StringLength(20)]
    public string Tipo { get; set; } 

    [StringLength(255)]
    public string Descripcion { get; set; }

    [Required]
    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    [Required]
    [Range(0.01, 9999.99)]
    public decimal Precio { get; set; }

    [Required]
    [StringLength(20)]
    public string Estado { get; set; }

    public Plataforma? Plataforma { get; set; }
    public Plan? Plan { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    [JsonIgnore]
    public ICollection<Miembro> Miembros { get; set; } = new List<Miembro>();
    [JsonIgnore]
    public ICollection<HistorialPago> HistorialPagos { get; set; } = new List<HistorialPago>();

    }
