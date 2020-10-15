using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [NotMapped]
    public class TurnoCambiarView
    {
        public Turno TurnoAntes { get; set; }
        public Turno TurnoDespues { get; set; }

    }
}