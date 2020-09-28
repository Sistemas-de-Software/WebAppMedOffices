using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("PacienteTurnos")]
    public class PacienteTurno
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Paciente")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Index("PacienteTurno_TurnoId_Index", IsUnique = true)]
        [Display(Name = "Turno")]
        public int TurnoId { get; set; }

        public virtual Paciente Paciente { get; set; }

        public virtual Turno Turno { get; set; }
    }
}