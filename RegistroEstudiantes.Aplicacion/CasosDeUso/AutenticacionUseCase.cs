using RegistroEstudiantes.Aplicacion.Puertos.Entrada;
using RegistroEstudiantes.Aplicacion.Puertos.Salida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Aplicacion.CasosDeUso
{
    public class AutenticacionUseCase : IAutenticacionUseCase
    {
        private readonly IAutenticacionRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AutenticacionUseCase(
            IAutenticacionRepository repository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<bool> RegistrarAsync(int idEstudiante, string nombre, string clave)
        {
            var claveHash = _passwordHasher.Hash(clave);

            return await _repository.RegistrarEstudianteAsync(
                idEstudiante,
                nombre,
                claveHash);
        }

        public async Task<string?> LoginAsync(int idEstudiante, string clave)
        {
            var estudiante = await _repository.ObtenerEstudiantePorIdAsync(idEstudiante);

            if (estudiante is null)
                return null;

            if (!_passwordHasher.Verificar(clave, estudiante.ClaveAcceso))
                return null;

            return _tokenService.GenerarToken(estudiante);
        }
    }
}
