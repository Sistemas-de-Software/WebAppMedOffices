using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebAppMedOffices.Shared;

namespace WebAppMedOffices.Models
{
    [Table("AtencionHorarios")]
    public class AtencionHorario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Consultorio")]
        public int ConsultorioId { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Día")]
        public Dia Dia { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de Inicio")]
        public DateTime HoraInicio { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de Fin")]
        public DateTime HoraFin { get; set; }

        public virtual Consultorio Consultorio { get; set; }
        
        public virtual Medico Medico { get; set; }
    }
}