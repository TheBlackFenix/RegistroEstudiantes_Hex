using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistroEstudiantes.Aplicacion.Puertos.Salida;
using RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Persistencia;
using RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Persistencia.Contexto;
using RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Infraestructura
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConexionBaseDeDatos")!;

            services.AddSingleton(new MySqlDbContext(connectionString));

            services.AddScoped<IAutenticacionRepository, AutenticacionRepository>();
            services.AddScoped<IEstudianteRepository, EstudianteRepository>();

            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}
