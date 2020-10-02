using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("Enfermedades")]
    public class Enfermedad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [MaxLength(50, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Index("Consultorio_Nombre_Index", IsUnique = true)]
        [Display(Name = "Enfermedad")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [Display(Name = "Tipo de Enfermedad")]
        public int TipoEnfermedadId { get; set; }

        public virtual TipoEnfermedad TipoEnfermedad { get; set; }

        public virtual ICollection<PacienteEnfermedad> Pacientes { get; set; }
    }
}