using ApiDoCesao.Data.Collections;
using ApiDoCesao.General;
using ApiDoCesao.Models;
using MongoDB.Driver;
using System.Linq;

namespace ApiDoCesao.Data.Business
{
    public class EnderecosBusiness
    {
        private readonly MongoDb _mongoDb;
        IMongoCollection<Enderecos> _enderecoCollection;

        public EnderecosBusiness(MongoDb databaseContext)
        {
            _mongoDb = databaseContext;
            _enderecoCollection = _mongoDb.MongoDatabaseContext.GetCollection<Enderecos>(typeof(Enderecos).Name.ToLower());
        }

        public void Salvar(EnderecoDto pEnderecoDto, Usuarios pUsuario)
        {
            var BdEndereco = _enderecoCollection.Find(
                    Builders<Enderecos>.Filter.Eq(end => end.UsuarioId, pUsuario.UsuarioId)
                ).FirstOrDefault();
            if (Utils.CepValido(pEnderecoDto.Cep) && BdEndereco == null)
            {
                var collectionName = _enderecoCollection.CollectionNamespace.CollectionName;
                var lEnderecoId = new SequenciasBusiness(_mongoDb).ProximoValor(collectionName);

                var lEndereco = new Enderecos
                (
                    pEnderecoDto.Numero,
                    pEnderecoDto.Rua,
                    pEnderecoDto.Cep
                );

                lEndereco.EnderecoId = lEnderecoId;
                lEndereco.UsuarioId = pUsuario.UsuarioId;

                _enderecoCollection.InsertOne(lEndereco);
            }
            else
            {
                throw new System.Exception("Não foi possivel salvar o endereço");
            }
        }

        public Enderecos EnderecoPorId(int pId)
        {
            return _enderecoCollection.Find(
                    Builders<Enderecos>.Filter.Eq(tel => tel.UsuarioId, pId)
                ).FirstOrDefault();
        }

        public Enderecos AlterarEndereco(int pId, EnderecoDto pEnderecoDto, Usuarios pUsuario)
        {
            var BdEndereco = _enderecoCollection.Find(
                    Builders<Enderecos>.Filter.Eq(end => end.UsuarioId, pUsuario.UsuarioId)
                ).FirstOrDefault();
            if (Utils.CepValido(pEnderecoDto.Cep) && BdEndereco != null)
            {
                return _enderecoCollection.FindOneAndUpdate(
                    Builders<Enderecos>.Filter.Eq(tel => tel.EnderecoId, pId),
                    Builders<Enderecos>.Update
                        .Set(tel => tel.EnderecoId, pId)
                        .Set(tel=>tel.UsuarioId,pUsuario.UsuarioId)
                        .Set(tel => tel.Numero, pEnderecoDto.Numero)
                        .Set(tel => tel.Cep, pEnderecoDto.Cep)
                        .Set(tel => tel.Rua, pEnderecoDto.Rua)
                );
            }
            else
            {
                throw new System.Exception("Não foi possivel alterar o endereço");
            }
        }

        public void DeletarEndereco(int pId)
        {
            try
            {
                _enderecoCollection.DeleteOne(
                    Builders<Enderecos>.Filter.Eq(tel => tel.UsuarioId, pId));
            }
            catch
            {
                throw new System.Exception("Não foi possivel deletar o endereço");
            }
        }
    }
}