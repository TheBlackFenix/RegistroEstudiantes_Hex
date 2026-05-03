using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.API.DTOs.Peticion;
using RegistroEstudiantes.API.DTOs.Respuesta;
using RegistroEstudiantes.Aplicacion.Puertos.Entrada;

namespace RegistroEstudiantes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionUseCase _autenticacionUseCase;

        public AutenticacionController(IAutenticacionUseCase autenticacionUseCase)
        {
            _autenticacionUseCase = autenticacionUseCase;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] RegistroEstudianteRequest request)
        {
            var registrado = await _autenticacionUseCase.RegistrarAsync(
                request.IdEstudiante,
                request.NombreEstudiante,
                request.ClaveAcceso);

            if (!registrado)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "No fue posible registrar el estudiante."
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Estudiante registrado correctamente."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _autenticacionUseCase.LoginAsync(
                request.IdEstudiante,
                request.Clave);

            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Credenciales inválidas."
                });
            }

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Inicio de sesión exitoso.",
                Token = token
            });
        }
    }
}
