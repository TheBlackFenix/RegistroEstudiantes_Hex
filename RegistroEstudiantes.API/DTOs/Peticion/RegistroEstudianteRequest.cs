namespace RegistroEstudiantes.API.DTOs.Peticion
{
    public class RegistroEstudianteRequest
    {
        public int IdEstudiante { get; set; }
        public string NombreEstudiante { get; set; } = null!;
        public string ClaveAcceso { get; set; } = null!;
    }
}
