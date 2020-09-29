using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [NotMapped]
    public class TurnoView : Turno
    {
        [Display(Name = "Fecha Desde")]
        public DateTime FechaDesde { get; set; }
        
        [Display(Name = "Fecha Hasta")]
        public DateTime FechaHasta { get; set; }
    }
}