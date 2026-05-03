using RegistroEstudiante.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Aplicacion.Puertos.Salida
{
    public interface IAutenticacionRepository
    {
        Task<bool> RegistrarEstudianteAsync(int idEstudiante, string nombre, string claveHash);
        Task<Estudiante?> ObtenerEstudiantePorIdAsync(int idEstudiante);
    }
}
