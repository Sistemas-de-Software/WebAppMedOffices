using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [NotMapped]
    public class LiquidacionObraSocialViewModel
    {
        public ObraSocial ObraSocial { get; set; }

        public ICollection<Turno> Turnos { get; set; }

        public decimal SubTotal { get; set; }
    }
}