using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Persistencia.Contexto
{
    public class MySqlDbContext
    {
        private readonly string _connectionString;

        public MySqlDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CrearConexion()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
