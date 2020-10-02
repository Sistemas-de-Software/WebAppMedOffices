using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("Especialidades")]
    public class Especialidad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} puede contener un máximo de {1} y un mínimo de {2} caracteres", MinimumLength = 3)]
        [Display(Name = "Especialidad")]
        public string Nombre { get; set; }

        public virtual ICollection<DuracionTurnoEspecialidad> DuracionTurnoEspecialidades { get; set; }

        public virtual ICollection<ObraSocialTarifa> Tarifas { get; set; }
        
        public virtual ICollection<Turno> Turnos { get; set; }
    }
}