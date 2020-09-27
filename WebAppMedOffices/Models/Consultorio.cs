using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("Consultorios")]
    public class Consultorio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [MaxLength(30, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Index("Consultorio_Nombre_Index", IsUnique = true)]
        [Display(Name = "Consultorio")]
        public string Nombre { get; set; }

        public virtual ICollection<AtencionHorario> AtencionHorarios { get; set; }
    }
}