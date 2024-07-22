using Microsoft.AspNetCore.Mvc;
using Produnet.WebBFF.ApiJoe.Dominio.DTO;
using Produnet.WebBFF.ApiJoe.Dominio.Modelos;
using Produnet.WebBFF.ApiJoe.Infraestructura.Repositorios.Interfaz;

namespace Produnet.WebBFF.ApiJoe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PersonasController: ControllerBase
    {
        private readonly IPersonaRepositorio personaRepositorio;
        
        /// <summary>
        /// Constructor de tipo PersonaController con un objeto Persona repositorio como parametro
        /// </summary>
        /// <param name="personaRepositorio"></param>
        public PersonasController(IPersonaRepositorio personaRepositorio)
        {
            this.personaRepositorio = personaRepositorio;            
        }


        /// <summary>
        /// Metodo para crear una nueva persona
        /// </summary>
        /// <param name="pedido">Datos para crear una nueva persona</param>
        /// <returns>Retorna la persona creada </returns>
        [HttpPost]
        public async Task<IActionResult> CrearPersona(CrearPersonaDto pedido)
        {
            var persona = new Persona
            {
                Nombre = pedido.Nombre,
                Apellido = pedido.Apellido,
                FechaNacimiento = pedido.FechaNacimiento
            };

            await personaRepositorio.CrearAsincronico(persona);

            var respuesta = new PersonaDto
            {
                Identificador = persona.Identificador,
                Nombre = persona.Nombre,
                Apellido = persona.Apellido,
                FechaNacimiento = persona.FechaNacimiento,
                Direccion = persona.Direccion,
                Telefono = persona.Telefono,
                Email = persona.Email
            };

            return Ok(respuesta);

        }


        /// <summary>
        /// Metodo para obtener a todas las personas
        /// </summary>
        /// <returns>Lista de todas las personas</returns>
        [HttpGet]
        public async Task <IActionResult> ObtenerPersonas()
        {
            var personas = await personaRepositorio.ObtenerTodosAsincronico();
            var respuesta = personas.Select(persona => new PersonaDto
            {
                Identificador = persona.Identificador,
                Nombre= persona.Nombre,
                Apellido= persona.Apellido,
                FechaNacimiento = persona.FechaNacimiento,
                Direccion= persona.Direccion,
                Telefono= persona.Telefono,
                Email = persona.Email
            });

            return Ok(respuesta);
        }

        /// <summary>
        /// Metodo para obtener a una Persona especificamente por su Identificador
        /// </summary>
        /// <param name="identificador">Identificador unico de la persona a buscar</param>
        /// <returns>Retorna el objeto Persona completo. NotFound si no encuentra</returns>
        [HttpGet("{identificador:guid}")]
        public async Task<IActionResult> ObtenerPersonaId(Guid identificador)
        {
            var persona = await personaRepositorio.ObtenerPorIdAsincronico(identificador);
            if (persona == null)
            {
                return NotFound();
            }

            var respuesta = new PersonaDto
            {
                Identificador = persona.Identificador,
                Nombre = persona.Nombre,
                Apellido = persona.Apellido,
                FechaNacimiento = persona.FechaNacimiento,
                Direccion = persona.Direccion,
                Telefono = persona.Telefono,
                Email = persona.Email

            };

            return Ok(respuesta);

        }

        /// <summary>
        /// Metodo para actualizar un objeto Persona por medio de su identificador y un objeto con los datos a actualizarse
        /// </summary>
        /// <param name="identificador">Identificador unico de la Persona</param>
        /// <param name="pedido">Objeto con los datos actualizados del objeto persona</param>
        /// <returns>Retorna la persona actualizada o NotFound si no se encuentra una persona con ese identificador.</returns>
        [HttpPut("{identificador:guid}")]
        public async Task<IActionResult> ActualizarPersona(Guid identificador, ActualizarPersonaDto pedido)
        {
            var personaExistente = await personaRepositorio.ObtenerPorIdAsincronico(identificador);
            if (personaExistente == null)
            {
                return NotFound();
            }

            personaExistente.Nombre = pedido.Nombre ?? personaExistente.Nombre;
            personaExistente.Apellido = pedido.Apellido ?? personaExistente.Apellido;
            personaExistente.FechaNacimiento = pedido.FechaNacimiento != default ? pedido.FechaNacimiento : personaExistente.FechaNacimiento;

            var personaActualizada = await personaRepositorio.ActualizarAsincronico(personaExistente);
            if (personaActualizada == null)
            {
                return NotFound();
            }

            var respuesta = new PersonaDto
            {
                Identificador = personaActualizada.Identificador,
                Nombre = personaActualizada.Nombre,
                Apellido = personaActualizada.Apellido,
                FechaNacimiento = personaActualizada.FechaNacimiento,
                Direccion = personaActualizada.Direccion,
                Telefono = personaActualizada.Telefono,
                Email = personaActualizada.Email
            };

            return Ok(respuesta);
        }

        /// <summary>
        /// Metodo para eliminar a una Persona por medio de su identificador
        /// </summary>
        /// <param name="identificador">Identificador unico de la Persona</param>
        /// <returns>NoContent si la eliminación es exitosa, o NotFound si no se encuentra a la persona con ese identificador</returns>
        [HttpDelete("{identificador:guid}")]
        public async Task<IActionResult> EliminarPersona(Guid identificador)
        {
            var personaEliminada = await personaRepositorio.EliminarAsincronico(identificador);
            if(personaEliminada == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
