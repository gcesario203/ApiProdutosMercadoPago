using ApiDoCesao.Data;
using ApiDoCesao.Data.Collections;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ApiDoCesao.General
{
    public static class TokenHandler
    {
        public static dynamic GerarToken(Usuarios pUsuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var chave = Encoding.ASCII.GetBytes(Settings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, pUsuario.Email),
                    new Claim(ClaimTypes.Role, pUsuario.Funcao)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString =  tokenHandler.WriteToken(token);

            return new
            {
                usuario = pUsuario,
                token = tokenString
            };
        }
    }
}
