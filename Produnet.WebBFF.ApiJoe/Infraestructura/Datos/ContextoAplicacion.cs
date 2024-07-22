using Microsoft.EntityFrameworkCore;
using Produnet.WebBFF.ApiJoe.Dominio.Modelos;
using System.Collections.Generic;

namespace Produnet.WebBFF.ApiJoe.Infraestructura.Datos
{
    public class ContextoAplicacion: DbContext
    {
        public ContextoAplicacion(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Persona> Personas { get; set; }
    }
}
