using MercadoPagoApi.Data.Collections;
using MercadoPagoApi.General;
using MercadoPagoApi.Models;
using MongoDB.Driver;
using System;
using System.Linq;

namespace MercadoPagoApi.Data.Business
{
    public class DocumentosBusiness
    {
        private readonly MongoDb _mongoDb;
        IMongoCollection<Documentos> _documentosCollection;

        public DocumentosBusiness(MongoDb databaseContext)
        {
            _mongoDb = databaseContext;
            _documentosCollection = _mongoDb.MongoDatabaseContext.GetCollection<Documentos>(typeof(Documentos).Name.ToLower());
        }

        public void Salvar(DocumentoDto pDocumentoDto, Usuarios pUsuario)
        {
            var BdDocumento = _documentosCollection.Find(
                    Builders<Documentos>.Filter.Eq(doc=>doc.UsuarioId,pUsuario.UsuarioId)
                ).FirstOrDefault();

            if (Utils.DocumentoValido(pDocumentoDto.Numero, pDocumentoDto.Tipo) && BdDocumento == null)
            {
                var collectionName = _documentosCollection.CollectionNamespace.CollectionName;
                var lDocumentoId = new SequenciasBusiness(_mongoDb).ProximoValor(collectionName);

                var lDocumento = new Documentos
                (
                    pDocumentoDto.Tipo,
                    pDocumentoDto.Numero
                );

                    lDocumento.DocumentoId = lDocumentoId;
                    lDocumento.UsuarioId = pUsuario.UsuarioId;


                _documentosCollection.InsertOne(lDocumento);
            }
            else
            {
                throw new Exception("Nao foi possivel salvar o documento");
            }
        }

        public Documentos DocumentoPorId(int pId)
        {
            try
            {
                return _documentosCollection.Find(
                    Builders<Documentos>.Filter.Eq(doc => doc.UsuarioId, pId)
                ).FirstOrDefault();
            }
            catch
            {
                throw new Exception("Documento não encontrado");
            }
        }

        public Documentos AlterarDocumento(int pId, DocumentoDto pDocumentoDto, Usuarios pUsuario)
        {
            var BdDocumento = _documentosCollection.Find(
                    Builders<Documentos>.Filter.Eq(doc => doc.UsuarioId, pUsuario.UsuarioId)
                ).FirstOrDefault();

            if (Utils.DocumentoValido(pDocumentoDto.Numero, pDocumentoDto.Tipo) && BdDocumento != null)
            {

                return _documentosCollection.FindOneAndUpdate(
                    Builders<Documentos>.Filter.Eq(doc => doc.DocumentoId, pId),
                    Builders<Documentos>.Update
                        .Set(doc => doc.DocumentoId, pId)
                        .Set(doc => doc.UsuarioId, pUsuario.UsuarioId)
                        .Set(doc => doc.Numero, pDocumentoDto.Numero)
                        .Set(doc => doc.Tipo, pDocumentoDto.Tipo)
                );
            }
            else
            {
                throw new System.Exception("Não foi possível editar o documento");
            }
                
        }

        public void DeletarDocumento(int pId)
        {
            try 
            {
               _documentosCollection.DeleteOne(
                 Builders<Documentos>.Filter.Eq(doc => doc.UsuarioId, pId));
            }
            catch 
            {
                throw new Exception("Não foi encontrado nenhum documento");
            }
        }
    }
}
