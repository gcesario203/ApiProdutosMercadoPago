using ApiDoCesao.Data.Collections;
using ApiDoCesao.General;
using ApiDoCesao.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiDoCesao.Data.Business
{
    public class AuthBusiness
    {
        private readonly MongoDb _mongoDb;

        IMongoCollection<Usuarios> _usuariosCollection;

        public AuthBusiness(MongoDb databaseContext)
        {
            _mongoDb = databaseContext;
            _usuariosCollection = _mongoDb.MongoDatabaseContext.GetCollection<Usuarios>(typeof(Usuarios).Name.ToLower());
        }

        public dynamic AutenticarUsuario(AuthDto pLoginDto)
        {
            var filtroComposto = Builders<Usuarios>.Filter.Eq(x => x.Email, pLoginDto.Email) & Builders<Usuarios>.Filter.Eq(x => x.Senha, pLoginDto.Senha);

            var lUsuario = _usuariosCollection.Find(filtroComposto).FirstOrDefault();

            if(lUsuario == null)
            {
                throw new Exception("Email ou senha incorreto");
            }

            var token = TokenHandler.GerarToken(lUsuario);

            return token;
        }
    }
}
