using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace WebAppMedOffices.Models
{
    [NotMapped]
    public class LiquidacionPacienteViewModel
    {
        public int PacienteId { get; set; }

        public ICollection<Turno> Turnos { get; set; }

        public decimal SubTotal { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Liquidación")]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Desde")]
        public DateTime FechaDesde { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Hasta")]
        public DateTime FechaHasta { get; set; }

    }
}