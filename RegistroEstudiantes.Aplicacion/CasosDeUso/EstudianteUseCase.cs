using RegistroEstudiante.Dominio.Entidades;
using RegistroEstudiantes.Aplicacion.Puertos.Entrada;
using RegistroEstudiantes.Aplicacion.Puertos.Salida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Aplicacion.CasosDeUso
{
    public class EstudianteUseCase : IEstudianteUseCase
    {
        private readonly IEstudianteRepository _repository;

        public EstudianteUseCase(IEstudianteRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> RegistrarMateriasAsync(int idEstudiante, List<int> materias)
        {
            return await _repository.RegistrarMateriasAsync(idEstudiante, materias);
        }

        public async Task<List<MateriaProfesor>> ObtenerMateriasDisponiblesAsync(int idEstudiante)
        {
            return await _repository.ObtenerMateriasDisponiblesAsync(idEstudiante);
        }

        public async Task<List<EstudianteMateria>> VerCompanerosAsync(int idEstudiante)
        {
            return await _repository.VerCompanerosAsync(idEstudiante);
        }

        public async Task<List<Materia>> ObtenerMateriasEstudianteAsync(int idEstudiante)
        {
            return await _repository.ObtenerMateriasEstudianteAsync(idEstudiante);
        }
    }
}
