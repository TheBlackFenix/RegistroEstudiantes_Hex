using RegistroEstudiante.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Aplicacion.Puertos.Salida
{
    public interface IEstudianteRepository
    {
        Task<bool> RegistrarMateriasAsync(int idEstudiante, List<int> materias);
        Task<List<MateriaProfesor>> ObtenerMateriasDisponiblesAsync(int idEstudiante);
        Task<List<EstudianteMateria>> VerCompanerosAsync(int idEstudiante);
        Task<List<Materia>> ObtenerMateriasEstudianteAsync(int idEstudiante);
    }
}
