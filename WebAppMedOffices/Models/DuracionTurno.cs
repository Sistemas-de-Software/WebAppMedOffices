using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("DuracionTurnoEspecialidades")]
    public class DuracionTurnoEspecialidad
    {
        [Key]
        public int Id { get; set; }

        [Index("DuracionTurnoEspecialidad_MedicoId_EspecialidadId_Index", IsUnique = true, Order = 1)]
        public int MedicoId { get; set; }

        [Index("DuracionTurnoEspecialidad_MedicoId_EspecialidadId_Index", IsUnique = true, Order = 2)]
        public int EspecialidadId { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [Display(Name = "Duración")]
        public int Duracion { get; set; }

        public virtual Medico Medico { get; set; }

        public virtual Especialidad Especialidad { get; set; }

    }
}