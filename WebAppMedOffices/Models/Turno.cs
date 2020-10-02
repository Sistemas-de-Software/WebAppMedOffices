using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebAppMedOffices.Shared;

namespace WebAppMedOffices.Models
{
    [Table("Turnos")]
    public class Turno
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }

        [Display(Name = "Paciente")]
        public int? PacienteId { get; set; }

        [Display(Name = "Obra Social")]
        public int? ObraSocialId { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Estado")]
        public Estado Estado { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha y Hora")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha y Hora Fin")]
        public DateTime FechaHoraFin { get; set; }

        [Display(Name = "Costo")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? Costo { get; set; }

        [Display(Name = "Sobreturno")]
        public bool? Sobreturno { get; set; }

        [Display(Name = "Tiene Obra Social")]
        public bool? TieneObraSocial { get; set; }

        public virtual Medico Medico { get; set; }

        public virtual Especialidad Especialidad { get; set; }
        
        public virtual Paciente Paciente { get; set; }
        
        public virtual ObraSocial ObraSocial { get; set; }
        
    }
}