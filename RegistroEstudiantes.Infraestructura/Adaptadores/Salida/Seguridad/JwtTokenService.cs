using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RegistroEstudiante.Dominio.Entidades;
using RegistroEstudiantes.Aplicacion.Puertos.Salida;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Infraestructura.Adaptadores.Salida.Seguridad
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Estudiante estudiante)
        {
            var claveSecreta = Encoding.UTF8.GetBytes(
                _configuration["Jwt:ClaveSecreta"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("id", estudiante.IdEstudiante.ToString()),
                new Claim("nombre", estudiante.NombreEstudiante)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(claveSecreta),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
