﻿using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Persistencia.Configuraciones
{
    //necesario para decirle a dbcontext como va a ser la relacion entre objetos ya que 1 cliente puede tener muchos casos o 
    public class CasoConfiguration : IEntityTypeConfiguration<Caso>
    {
        public void Configure(EntityTypeBuilder<Caso> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Cliente)
                   .WithMany(cl => cl.Casos)
                   .HasForeignKey(c => c.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
