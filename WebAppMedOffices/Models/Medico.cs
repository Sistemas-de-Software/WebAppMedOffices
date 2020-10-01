using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("Medicos")]
    public class Medico
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} puede contener un máximo de {1} y un mínimo de {2} caracteres", MinimumLength = 3)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} puede contener un máximo de {1} y un mínimo de {2} caracteres", MinimumLength = 3)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} puede contener un máximo de {1} y un mínimo de {2} caracteres", MinimumLength = 3)]
        [Index("Medico_UserName_Index", IsUnique = true)]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [MaxLength(50, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Index("Medico_Matricula_Index", IsUnique = true)]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Display(Name = "Nombre y Apellido")]
        public virtual string NombreCompleto
        {
            get
            {
                return Nombre + " " + Apellido;
            }
        }

        public virtual ICollection<DuracionTurnoEspecialidad> DuracionTurnoEspecialidades { get; set; }

        public virtual ICollection<AtencionHorario> AtencionHorarios { get; set; }
        
        public virtual ICollection<Turno> Turnos { get; set; }
        

    }
}