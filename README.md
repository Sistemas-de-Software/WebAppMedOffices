# WebAppMedOffices

## Usuarios y Roles de Prueba

### Roles 
Medico
Secretaria
Admin

### Usuarios y pass
admin@medoffices.com Admin1234@

secretaria@medoffices.com Secretaria1234@

medico1@medoffices.com Medico11234@

medico2@medoffices.com Medico21234@

medico3@medoffices.com Medico31234@


## Data Annotations
> Guía de Code First Data Annotations
https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations

## Enable Migrations

> Carpeta: Migrations/Configuration.cs

Se pone `AutomaticMigrationsEnabled = true;` solo para habilitar las migraciones automáticas, eso quiere
decir que en la consola Package Manage solo debemos correr el comando `Update-Database` cuando hacemos cambios en el modelo

```c#
internal sealed class Configuration : DbMigrationsConfiguration<WebAppMedOffices.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "WebAppMedOffices.Models.ApplicationDbContext";
        }

        protected override void Seed(WebAppMedOffices.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
```

## Ejemplo de Modelo

```c#
    [Table("Medicos")] // Nombre de la tabla en la Base de Datos
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

    }
```

## Problemas Típicos
### No se pudo encontrar una parte de la ruta… bin \ roslyn \ csc.exe
SOLUCIÓN ejecute esto en la Consola del Administrador de paquetes:

```
Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
```

Fuente: https://stackoverflow.com/questions/32780315/could-not-find-a-part-of-the-path-bin-roslyn-csc-exe

### Error ¿Cómo veo la propiedad 'EntityValidationErrors' desde la consola del paquete nuget?

https://qastack.mx/programming/10219864/ef-code-first-how-do-i-see-entityvalidationerrors-property-from-the-nuget-pac
