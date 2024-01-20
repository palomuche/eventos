using EventoCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventoCore.Context
{
    public class ApplicationContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                if (entity.GetTableName() == "AspNetUsers")
                {
                    entity.SetTableName("Usuarios");
                }
                if (entity.GetTableName() == "AspNetUserClaims")
                {
                    entity.SetTableName("UsuarioAcessos");
                }
                if (entity.GetTableName() == "AspNetRoleClaims")
                {
                    entity.SetTableName("FuncaoAcessos");
                }
                if (entity.GetTableName() == "AspNetUserLogins")
                {
                    entity.SetTableName("UsuarioLogins");
                }
                if (entity.GetTableName() == "AspNetRoles")
                {
                    entity.SetTableName("Funcoes");
                }
                if (entity.GetTableName() == "AspNetUserRoles")
                {
                    entity.SetTableName("UsuarioFuncoes");
                }
                if (entity.GetTableName() == "AspNetUserTokens")
                {
                    entity.SetTableName("UsuarioTokens");
                }
            }
        }
    }
}
