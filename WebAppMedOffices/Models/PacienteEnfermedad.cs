using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("PacienteEnfermedades")]
    public class PacienteEnfermedad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Index("PacienteEnfermedad_PacienteId_EnfermedadId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Paciente")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [Index("PacienteEnfermedad_PacienteId_EnfermedadId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "Enfermedad")]
        public int EnfermedadId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public virtual Paciente Paciente { get; set; }

        public virtual Enfermedad Enfermedad { get; set; }
    }
}