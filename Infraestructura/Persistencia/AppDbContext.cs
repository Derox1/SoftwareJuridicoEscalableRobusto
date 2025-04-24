using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades;

namespace Infraestructura.Persistencia
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Caso> Casos { get; set; } 
        public DbSet<Cliente> Clientes { get; set; }


        //este codigo es para
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<Caso>()   // 1º Bloque: Relaciones
            .HasOne(c => c.Cliente)
            .WithMany(cl => cl.Casos)
            .HasForeignKey(c => c.ClienteId)
             .OnDelete(DeleteBehavior.Restrict);

            //separados siempre 
             modelBuilder.Entity<Caso>() // 2º Bloque: Configuración de propiedades
             .Property(c => c.Estado)
             .HasConversion<string>();
        }
    }
}
