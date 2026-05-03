using Microsoft.Extensions.DependencyInjection;
using RegistroEstudiantes.Aplicacion.CasosDeUso;
using RegistroEstudiantes.Aplicacion.Puertos.Entrada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Aplicacion
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAutenticacionUseCase, AutenticacionUseCase>();
            services.AddScoped<IEstudianteUseCase, EstudianteUseCase>();

            return services;
        }
    }
}
