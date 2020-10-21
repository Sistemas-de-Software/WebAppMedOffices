using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [NotMapped]
    public class PacienteEnfermedadView : PacienteEnfermedad
    {
        public int TurnoId { get; set; }
    }
}