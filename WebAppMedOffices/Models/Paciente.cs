﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Models
{
    [Table("Pacientes")]
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [MaxLength(50, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [MaxLength(50, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }
            
        [Required(ErrorMessage = "Debes introducir un {0}")]
        [MaxLength(20, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Index("Consultorio_Nombre_Index", IsUnique = true)]
        [Display(Name = "Documento")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Debes introducir una {0}")]
        [MaxLength(120, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "No es un número de teléfono válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "No es un número de teléfono válido")]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [Display(Name = "Obra Social")]
        public int? ObraSocialId { get; set; }

        [StringLength(30, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Número de afiliado")]
        public string NroAfiliado { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} puede contener un máximo de {1} caracteres")]
        [Display(Name = "Contacto de Emergencia")]
        public string NombreContactoEmergencia { get; set; }
        
        [StringLength(30, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono de Emergencia")]
        public string TelefonoContactoEmergencia { get; set; }

        [Required(ErrorMessage = "Debes introducir un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} puede contener un máximo de {1} y un mínimo de {2} caracteres", MinimumLength = 3)]        
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Mail { get; set; }

        [Display(Name = "Nombre y Apellido")]
        public virtual string NombreCompleto
        {
            get
            {
                return Nombre + " " + Apellido;
            }
        }


        public virtual ObraSocial ObraSocial { get; set; }

        public virtual ICollection<PacienteEnfermedad> Enfermedades { get; set; }
        
        public virtual ICollection<Turno> Turnos { get; set; }
    }
}