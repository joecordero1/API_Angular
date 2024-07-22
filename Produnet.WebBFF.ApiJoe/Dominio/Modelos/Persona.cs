using System.ComponentModel.DataAnnotations;

namespace Produnet.WebBFF.ApiJoe.Dominio.Modelos
{

    public class Persona
    {
        [Key]
        public Guid Identificador { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }
}
