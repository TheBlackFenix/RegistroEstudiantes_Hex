using Dapper;
using RegistroEstudiante.Dominio.Entidades;
using RegistroEstudiantes.Aplicacion.Puertos.Salida;
using RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Persistencia.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Persistencia
{
    public class AutenticacionRepository : IAutenticacionRepository
    {
        private readonly MySqlDbContext _connectionFactory;

        public AutenticacionRepository(MySqlDbContext connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> RegistrarEstudianteAsync(int idEstudiante, string nombre, string claveHash)
        {
            using var conn = _connectionFactory.CrearConexion();

            var parametros = new DynamicParameters();
            parametros.Add("p_IdEstudiante", idEstudiante);
            parametros.Add("p_Nombre", nombre);
            parametros.Add("p_ClaveAcceso", claveHash);

            await conn.ExecuteAsync(
                "sp_RegistrarEstudiante",
                parametros,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<Estudiante?> ObtenerEstudiantePorIdAsync(int idEstudiante)
        {
            using var conn = _connectionFactory.CrearConexion();

            return await conn.QueryFirstOrDefaultAsync<Estudiante>(
                "sp_LoginEstudiante",
                new { p_IdEstudiante = idEstudiante },
                commandType: CommandType.StoredProcedure);
        }
    }
}
