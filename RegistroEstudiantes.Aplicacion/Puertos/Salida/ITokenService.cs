using RegistroEstudiante.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Aplicacion.Puertos.Salida
{
    public interface ITokenService
    {
        string GenerarToken(Estudiante estudiante);
    }
}
