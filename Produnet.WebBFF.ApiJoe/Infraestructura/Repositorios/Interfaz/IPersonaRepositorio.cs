using Produnet.WebBFF.ApiJoe.Infraestructura.Repositorios.Interfaz;
using Produnet.WebBFF.ApiJoe.Infraestructura.Datos;
using Produnet.WebBFF.ApiJoe.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;


namespace Produnet.WebBFF.ApiJoe.Infraestructura.Repositorios.Interfaz
{
    public interface IPersonaRepositorio
    {
        Task<Persona> CrearAsincronico(Persona persona);
        Task<IEnumerable<Persona>> ObtenerTodosAsincronico();
        Task<Persona> ObtenerPorIdAsincronico(Guid id);
        Task<Persona> ActualizarAsincronico(Persona persona);
        Task<Persona> EliminarAsincronico(Guid id);
    }
}
