using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.API.DTOs.Peticion;
using RegistroEstudiantes.API.DTOs.Respuesta;
using RegistroEstudiantes.Aplicacion.Puertos.Entrada;

namespace RegistroEstudiantes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteUseCase _estudianteUseCase;

        public EstudiantesController(IEstudianteUseCase estudianteUseCase)
        {
            _estudianteUseCase = estudianteUseCase;
        }

        [HttpGet("materias-disponibles")]
        public async Task<IActionResult> ObtenerMateriasDisponibles()
        {
            if (!TryGetIdEstudiante(out var idEstudiante))
                return Unauthorized();

            var materias = await _estudianteUseCase.ObtenerMateriasDisponiblesAsync(idEstudiante);

            return Ok(materias);
        }

        [HttpPost("registrar-materias")]
        public async Task<IActionResult> RegistrarMaterias([FromBody] RegistrarMateriasRequest request)
        {
            if (!TryGetIdEstudiante(out var idEstudiante))
                return Unauthorized();

            var registrado = await _estudianteUseCase.RegistrarMateriasAsync(
                idEstudiante,
                request.Materias);

            if (!registrado)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "No fue posible registrar las materias."
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Materias registradas correctamente."
            });
        }

        [HttpGet("companeros")]
        public async Task<IActionResult> VerCompaneros()
        {
            if (!TryGetIdEstudiante(out var idEstudiante))
                return Unauthorized();

            var companeros = await _estudianteUseCase.VerCompanerosAsync(idEstudiante);

            return Ok(companeros);
        }

        [HttpGet("materias-inscritas")]
        public async Task<IActionResult> ObtenerMateriasInscritas()
        {
            if (!TryGetIdEstudiante(out var idEstudiante))
                return Unauthorized();

            var materias = await _estudianteUseCase.ObtenerMateriasEstudianteAsync(idEstudiante);

            return Ok(materias);
        }

        private bool TryGetIdEstudiante(out int idEstudiante)
        {
            idEstudiante = 0;

            var claim = User.FindFirst("id");

            return claim is not null &&
                   int.TryParse(claim.Value, out idEstudiante) &&
                   idEstudiante > 0;
        }
    }
}
