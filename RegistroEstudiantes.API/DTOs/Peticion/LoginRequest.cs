namespace RegistroEstudiantes.API.DTOs.Peticion
{
    public class LoginRequest
    {
        public int IdEstudiante { get; set; }
        public string Clave { get; set; } = null!;
    }
}
