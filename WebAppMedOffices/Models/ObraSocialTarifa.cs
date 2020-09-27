using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("ObraSocialTarifas")]
    public class ObraSocialTarifa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Tarifa")]
        public decimal Tarifa { get; set; }

        [Display(Name = "Obra Social")]
        public int ObraSocialId { get; set; }

        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }

        public virtual ObraSocial ObraSocial { get; set; }

        public virtual Especialidad Especialidad { get; set; }
    }
}