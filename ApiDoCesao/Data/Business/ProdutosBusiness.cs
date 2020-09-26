using ApiDoCesao.Data.Collections;
using ApiDoCesao.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ApiDoCesao.Data.Business
{
    public class ProdutosBusiness
    {
        private readonly MongoDb _mongoDb;

        IMongoCollection<Produtos> _produtosCollection;

        public ProdutosBusiness(MongoDb databaseContext)
        {
            _mongoDb = databaseContext;
            _produtosCollection = _mongoDb.MongoDatabaseContext.GetCollection<Produtos>(typeof(Produtos).Name.ToLower());
        }

        public void Salvar(ProdutoDto pProdutoDto)
        {
                var collectionName = _produtosCollection.CollectionNamespace.CollectionName;
                var lProdutoId = new SequenciasBusiness(_mongoDb).ProximoValor(collectionName);

                var lProduto = new Produtos
                (
                    lProdutoId,
                    pProdutoDto.Nome,
                    pProdutoDto.Preco
                );

                _produtosCollection.InsertOne(lProduto);
        }

        public List<Produtos> Todos()
        {
                return _produtosCollection.Find(
                        Builders<Produtos>.Filter.Empty
                    ).ToList();
        }

        public Produtos ProdutoPorId(int pId)
        {
                return _produtosCollection.Find(
                        Builders<Produtos>.Filter.Eq(prod => prod.ProdutoId, pId)
                    ).FirstOrDefault();
        }

        public Produtos AlterarProduto(int pId, ProdutoDto pProdutoDto)
        {
                return _produtosCollection.FindOneAndUpdate(
                    Builders<Produtos>.Filter.Eq(prod => prod.ProdutoId, pId),
                    Builders<Produtos>.Update
                        .Set(prod => prod.ProdutoId, pId)
                        .Set(prod => prod.Nome, pProdutoDto.Nome)
                        .Set(prod => prod.Preco, pProdutoDto.Preco)
                    );
        }

        public void DeletarProduto(int pId)
        {
            _produtosCollection.DeleteOne(
                    Builders<Produtos>.Filter.Eq(prod => prod.ProdutoId, pId));
        }
    }
}