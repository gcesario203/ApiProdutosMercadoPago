using MercadoPagoApi.Data.Collections;
using MercadoPagoApi.General;
using MercadoPagoApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MercadoPagoApi.Data.Business
{
    public class TelefonesBusiness
    {
        private readonly MongoDb _mongoDb;
        IMongoCollection<Telefones> _telefonesCollection;

        public TelefonesBusiness(MongoDb databaseContext)
        {
            _mongoDb = databaseContext;
            _telefonesCollection = _mongoDb.MongoDatabaseContext.GetCollection<Telefones>(typeof(Telefones).Name.ToLower());
        }

        public void Salvar(TelefoneDto pTelefoneDto, Usuarios pUsuario)
        {
            if (Utils.TelValido(pTelefoneDto.Area, pTelefoneDto.Numero))
            {
                var collectionName = _telefonesCollection.CollectionNamespace.CollectionName;
                var lTelefoneId = new SequenciasBusiness(_mongoDb).ProximoValor(collectionName);

                var lTelefone = new Telefones
                (
                    pTelefoneDto.Area,
                    pTelefoneDto.Numero
                );

                lTelefone.TelefoneId = lTelefoneId;
                lTelefone.UsuarioId = pUsuario.UsuarioId;

                _telefonesCollection.InsertOne(lTelefone);
            }
            else
            {
                throw new Exception("Não foi possivel salvar os telefones");
            }
        }

        public Telefones TelefonePorId(int pId)
        {
            return _telefonesCollection.Find(
                    Builders<Telefones>.Filter.Eq(tel => tel.UsuarioId, pId)
                ).FirstOrDefault();
        }

        public Telefones AlterarTelefone(int pId, TelefoneDto pTelefoneDto)
        {
            var BdTelefone = _telefonesCollection.Find(
                    Builders<Telefones>.Filter.Eq(end => end.UsuarioId, pId)
                ).FirstOrDefault();

            if (Utils.TelValido(pTelefoneDto.Area, pTelefoneDto.Numero) && BdTelefone != null)
            {
                 return _telefonesCollection.FindOneAndUpdate(
                    Builders<Telefones>.Filter.Eq(tel => tel.UsuarioId, pId),
                    Builders<Telefones>.Update
                        .Set(tel => tel.TelefoneId, pId)
                        .Set(tel => tel.Numero, pTelefoneDto.Numero)
                        .Set(tel => tel.Area, pTelefoneDto.Area)
                );
            }
            else
            {
                throw new System.Exception("Não foi possivel alterar o telefone");
            }
        }

        public void DeletarTelefone(Usuarios pUsuario)
        {
            try
            {
                    _telefonesCollection.DeleteOne(
                        Builders<Telefones>.Filter.Eq(tel => tel, pUsuario.Telefone)
                     );

            }
            catch
            {
                throw new Exception("Não foi possivel deletar os telefones");
            }
        }
    }
}
