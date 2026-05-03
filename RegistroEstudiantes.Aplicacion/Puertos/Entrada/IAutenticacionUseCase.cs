using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Aplicacion.Puertos.Entrada
{
    public interface IAutenticacionUseCase
    {
        Task<bool> RegistrarAsync(int idEstudiante, string nombre, string clave);
        Task<string?> LoginAsync(int idEstudiante, string clave);
    }
}
