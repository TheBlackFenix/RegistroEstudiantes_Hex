namespace RegistroEstudiantes.API.DTOs.Respuesta
{
    public class LoginResponse : ApiResponse
    {
        public string Token { get; set; } = null!;
    }
}
