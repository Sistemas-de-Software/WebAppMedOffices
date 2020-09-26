using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("Consultorio")]
    public class Consultorio
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Consultorio")]
        public string Nombre { get; set; }
    }
}