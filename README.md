# WebAppMedOffices

Guía de Code First Data Annotations
https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations

## Enable Migrations

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

