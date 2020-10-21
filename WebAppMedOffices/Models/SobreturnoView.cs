using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [NotMapped]
    public class SobreturnoView
    {
        public DateTime Fecha { get; set; }
        public IEnumerable<Turno> Turnos { get; set; }
    }
}