using Dapper;
using RegistroEstudiante.Dominio.Entidades;
using RegistroEstudiantes.Aplicacion.Puertos.Salida;
using RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Persistencia.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Persistencia
{
    public class EstudianteRepository : IEstudianteRepository
    {

        private readonly MySqlDbContext _contexto;

        public EstudianteRepository(MySqlDbContext connectionFactory)
        {
            _contexto = connectionFactory;
        }
        public async Task<bool> RegistrarMateriasAsync(int idEstudiante, List<int> materias)
        {
            using var conn = _contexto.CrearConexion();
            var jsonMaterias = JsonSerializer.Serialize(materias);
            var parametros = new DynamicParameters();
            parametros.Add("p_IdEstudiante", idEstudiante);
            parametros.Add("p_MateriasJSON", jsonMaterias);

            try
            {
                await conn.ExecuteAsync("sp_RegistrarMateriasEstudiante", parametros, commandType: CommandType.StoredProcedure);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<MateriaProfesor>> ObtenerMateriasDisponiblesAsync(int idEstudiante)
        {
            using var conn = _contexto.CrearConexion();
            var resultado = await conn.QueryAsync<MateriaProfesor>(
                "sp_ObtenerMateriasDisponibles",
                new { p_IdEstudiante = idEstudiante },
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        public async Task<List<EstudianteMateria>> VerCompanerosAsync(int idEstudiante)
        {
            using var conn = _contexto.CrearConexion();
            var resultado = await conn.QueryAsync<EstudianteMateria>(
                "sp_VerEstudiantesPorMateria",
                new { p_IdEstudiante = idEstudiante },
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        public async Task<List<Materia>> ObtenerMateriasEstudianteAsync(int idEstudiante)
        {
            using var conn = _contexto.CrearConexion();
            var resultado = await conn.QueryAsync<Materia>(
                "sp_ObtenerMateriasRegistradas",
                new { p_IdEstudiante = idEstudiante },
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

    }
}
