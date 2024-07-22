using Microsoft.EntityFrameworkCore;
using Produnet.WebBFF.ApiJoe.Dominio.Modelos;
using Produnet.WebBFF.ApiJoe.Infraestructura.Datos;
using Produnet.WebBFF.ApiJoe.Infraestructura.Repositorios.Interfaz;

namespace Produnet.WebBFF.ApiJoe.Infraestructura.Repositorios.Implementacion
{
    /// <summary>
    /// Repositorio para manejar operaciones CRUD de la entidad Persona.
    /// </summary>
    public class PersonaRepositorio : IPersonaRepositorio
    {
        private readonly ContextoAplicacion contexto;
        /// <summary>
        /// Inicializa una nueva instancia de PersonaRepositorio con el contexto de la aplicación
        /// </summary>
        /// <param name="contexto">Contexto de la aplicación.</param>
        public PersonaRepositorio(ContextoAplicacion contexto)
        {
            this.contexto = contexto;
        }

        /// <summary>
        /// Crea una nueva persona de forma asíncrona.
        /// </summary>
        /// <param name="persona">Objeto de tipo persona a crearse</param>
        /// <returns>Objeto de tipo Persona creado.</returns>
        public async Task<Persona> CrearAsincronico(Persona persona)
        {
            if (string.IsNullOrEmpty(persona.Direccion) || string.IsNullOrEmpty(persona.Email) || string.IsNullOrEmpty(persona.Telefono))
            {
                persona.Direccion = "Dirección no proporcionada";
                persona.Telefono = "Teléfono no proporcionado";
                persona.Email = "Email no proporcionado";
            }
            await contexto.Personas.AddAsync(persona);
            await contexto.SaveChangesAsync();

            return persona;
        }

        /// <summary>
        /// Obtiene todas las personas de forma asíncrona.
        /// </summary>
        /// <returns>Lista de todas las personas.</returns>
        public async Task<IEnumerable<Persona>> ObtenerTodosAsincronico()
        {
            return await contexto.Personas.ToListAsync();
        }

        /// <summary>
        /// Obtiene una persona especifica por medio de su id de forma asincronica
        /// </summary>
        /// <param name="id">Identificador de la persona</param>
        /// <returns>Retorna objeto de tipo Persona. Null si no encuentra</returns>
        public async Task<Persona> ObtenerPorIdAsincronico(Guid id)
        {
            return await contexto.Personas.FindAsync(id);
        }

        /// <summary>
        /// Metodo asincronico para actualizar un objeto Persona existente
        /// </summary>
        /// <param name="persona">Objeto persona con la informacion actualizada</param>
        /// <returns> Retorna el objeto Persona ya actualizado</returns>
        public async Task<Persona> ActualizarAsincronico(Persona persona)
        {
            var personaExistente = await contexto.Personas.FindAsync(persona.Identificador);
            if (personaExistente == null)
            {
                return null;
            }

            personaExistente.Nombre = persona.Nombre;
            personaExistente.Apellido = persona.Apellido;
            personaExistente.FechaNacimiento = persona.FechaNacimiento;

            if (!string.IsNullOrEmpty(persona.Direccion) || !string.IsNullOrEmpty(persona.Telefono) || !string.IsNullOrEmpty(persona.Telefono) || !string.IsNullOrEmpty(persona.Email))
            {
                personaExistente.Direccion = persona.Direccion;
                personaExistente.Telefono = persona.Telefono;
                personaExistente.Email = persona.Email;
            }

            await contexto.SaveChangesAsync();
            return personaExistente;
        }

        /// <summary>
        /// Metodo asincronico para eliminar un objeto Persona existente por medio de su identificador
        /// </summary>
        /// <param name="identificador">Identificador del objeto Persona</param>
        /// <returns>Objeto Persona eliminado, null si no se encuentra.</returns>
        public async Task<Persona> EliminarAsincronico(Guid identificador)
        {
            var personaExistente = await contexto.Personas.FindAsync(identificador);
            if (personaExistente == null)
            {
                return null;
            }

            contexto.Personas.Remove(personaExistente);
            await contexto.SaveChangesAsync();
            return personaExistente;
        }
    }
}

