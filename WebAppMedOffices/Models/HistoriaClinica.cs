using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebAppMedOffices.Shared;

namespace WebAppMedOffices.Models
{
    [Table("HistoriaClinicas")]
    public class HistoriaClinica
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Paciente")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Turno")]
        public int TurnoId { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Motivo")]
        public Motivo Motivo { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [MaxLength(50, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Display(Name = "Detalle")]
        public string Detalle { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [MaxLength(50, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Display(Name = "Comentario")]
        public string Comentario { get; set; }

        public virtual Paciente Paciente { get; set; }
        public virtual Turno Turno { get; set; }
    }
}