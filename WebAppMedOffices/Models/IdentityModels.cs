using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebAppMedOffices.Migrations;

namespace WebAppMedOffices.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            if (this.FirstName != null && this.LastName != null)
            {
                userIdentity.AddClaim(new Claim("FirstName", this.FirstName));
                userIdentity.AddClaim(new Claim("LastName", this.LastName));
            }
            return userIdentity;
        }
    }

    public static class Utilities
    {
        public static string GetFirstName(this System.Security.Principal.IPrincipal usr)
        {
            var firstNameClaim = ((ClaimsIdentity)usr.Identity).FindFirst("FirstName");
            if (firstNameClaim != null)
                return firstNameClaim.Value;

            return "";
        }

        public static string GetLastName(this System.Security.Principal.IPrincipal usr)
        {
            var lastNameClaim = ((ClaimsIdentity)usr.Identity).FindFirst("LastName");
            if (lastNameClaim != null)
                return lastNameClaim.Value;

            return "";
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Dahabilitar el borrado en cascada
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Consultorio> Consultorios { get; set; }
        public DbSet<DuracionTurnoEspecialidad> DuracionTurnoEspecialidades { get; set; }
        public DbSet<AtencionHorario> AtencionHorarios { get; set; }
        public DbSet<ObraSocial> ObrasSociales { get; set; }
        public DbSet<ObraSocialTarifa> ObraSocialTarifas { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<TipoEnfermedad> TipoEnfermedades { get; set; }
        public DbSet<Enfermedad> Enfermedades { get; set; }
        public DbSet<PacienteEnfermedad> PacienteEnfermedades { get; set; }
        public DbSet<HistoriaClinica> HistoriaClinicas { get; set; }

    }
}