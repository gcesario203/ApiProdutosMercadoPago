using ApiDoCesao.Data.Collections;
using ApiDoCesao.General;
using ApiDoCesao.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDoCesao.Data.Business
{
    public class UsuariosBusiness
    {
        private readonly MongoDb _mongoDb;

        IMongoCollection<Usuarios> _usuariosCollection;
        private DocumentosBusiness _documentosBusiness { get; set; }
        private EnderecosBusiness _enderecosBusiness { get; set; }
        private TelefonesBusiness _telefonesBusiness { get; set; }


        public UsuariosBusiness(MongoDb databaseContext)
        {
            _mongoDb = databaseContext;
            _usuariosCollection = _mongoDb.MongoDatabaseContext.GetCollection<Usuarios>(typeof(Usuarios).Name.ToLower());
            _documentosBusiness = new DocumentosBusiness(databaseContext);
            _enderecosBusiness = new EnderecosBusiness(databaseContext);
            _telefonesBusiness = new TelefonesBusiness(databaseContext);
        }

        public void Salvar(UsuarioDto pUsuarioDto)
        {
            var checarEmail = UsuarioPorEmail(pUsuarioDto.Login.Email);

            if (checarEmail != null)
            {
                throw new Exception("Email já cadastrado");
            }
            else
            {
                if (pUsuarioDto.Login.Senha == pUsuarioDto.SenhaConfirmacao)
                {
                    if (Utils.SenhaValida(pUsuarioDto.Login.Senha) && Utils.EmailValido(pUsuarioDto.Login.Email))
                    {
                        var lUsuario = new Usuarios
                        (
                            pUsuarioDto.Nome,
                            pUsuarioDto.Sobrenome,
                            pUsuarioDto.Login.Email,
                            pUsuarioDto.Login.Senha,
                            "user"

                        );

                        if (lUsuario != null)
                        {
                            var collectionName = _usuariosCollection.CollectionNamespace.CollectionName;
                            var lUsuarioId = new SequenciasBusiness(_mongoDb).ProximoValor(collectionName);

                            lUsuario.UsuarioId = lUsuarioId;

                            _documentosBusiness.Salvar(pUsuarioDto.Documento, lUsuario);
                            _enderecosBusiness.Salvar(pUsuarioDto.Endereco, lUsuario);
                            _telefonesBusiness.Salvar(pUsuarioDto.Telefone, lUsuario);

                            var documento = _documentosBusiness.DocumentoPorId(lUsuario.UsuarioId);
                            var endereco = _enderecosBusiness.EnderecoPorId(lUsuario.UsuarioId);
                            var telefone = _telefonesBusiness.TelefonePorId(lUsuario.UsuarioId);

                            if (documento != null && endereco != null && telefone != null)
                            {
                                lUsuario.Documento = documento;
                                lUsuario.Endereco = endereco;
                                lUsuario.Telefone = telefone;

                                _usuariosCollection.InsertOne(lUsuario);
                            }
                            else
                            {
                                _documentosBusiness.DeletarDocumento(lUsuario.UsuarioId);
                                _enderecosBusiness.DeletarEndereco(lUsuario.UsuarioId);
                                _telefonesBusiness.DeletarTelefone(lUsuario);

                                throw new Exception("Não foi possivel finalizar a operação");
                            }
                        }
                        else
                        {
                            throw new Exception("Erro ao cadastrar usuário");
                        }
                    }
                    else
                    {
                        throw new Exception("Formato de email ou senha invalidos");
                    }

                }
                else
                {
                    throw new Exception("Senhas não se coincidem");
                }
            }
        }

        public List<Usuarios> MostrarTodosUsuarios()
        {
            return _usuariosCollection.Find(
                    Builders<Usuarios>.Filter.Empty
                    ).ToList();
        }

        public Usuarios UsuarioPorId(int pId)
        {
            return _usuariosCollection.Find(Builders<Usuarios>.Filter.Eq(usr => usr.UsuarioId, pId)).FirstOrDefault();
        }

        public Usuarios UsuarioPorEmail(string pEmail)
        {
            return _usuariosCollection.Find(Builders<Usuarios>.Filter.Eq(usr => usr.Email, pEmail)).FirstOrDefault();
        }

        public void AlterarUsuario(int pId, UsuarioDto pUsuario)
        {
            var BdUsuario = UsuarioPorId(pId);

            if (pUsuario.Login.Senha == pUsuario.SenhaConfirmacao && BdUsuario != null)
            {
                if (Utils.SenhaValida(pUsuario.Login.Senha) && Utils.EmailValido(pUsuario.Login.Email)) 
                {
                    try
                    {
                        var documentoMudado =_documentosBusiness.AlterarDocumento(BdUsuario.UsuarioId, pUsuario.Documento, BdUsuario);
                        var enderecoMudado = _enderecosBusiness.AlterarEndereco(BdUsuario.Endereco.UsuarioId, pUsuario.Endereco, BdUsuario);
                        var telefoneMudado = _telefonesBusiness.AlterarTelefone(BdUsuario.UsuarioId, pUsuario.Telefone);

                        _usuariosCollection.FindOneAndUpdate(
                            Builders<Usuarios>.Filter.Eq(usr => usr.UsuarioId, pId),
                            Builders<Usuarios>.Update
                                .Set(usr => usr.UsuarioId, pId)
                                .Set(usr => usr.Nome, pUsuario.Nome)
                                .Set(usr => usr.Sobrenome, pUsuario.Sobrenome)
                                .Set(usr => usr.Senha, pUsuario.Login.Senha)
                                .Set(usr => usr.Email, pUsuario.Login.Email)
                                .Set(usr =>usr.Documento, documentoMudado)
                                .Set(usr=>usr.Endereco,enderecoMudado)
                                .Set(usr => usr.Telefone, telefoneMudado)
                            );
                    }
                    catch(Exception err)
                    {
                        throw new Exception(err.Message);
                    }
                }
            }
        }

        public void AtualizarCarrinho(Usuarios pUsuario)
        {
            try
            {
                _usuariosCollection.UpdateOne(Builders<Usuarios>.Filter.Eq(usr => usr.UsuarioId, pUsuario.UsuarioId), Builders<Usuarios>.Update.Set(usr => usr.CarrinhoDeCompras, pUsuario.CarrinhoDeCompras));
            }
            catch
            {
                throw new Exception("Não foi possivel atualizar o carrinho"); 
            }
            
        }

        public void DeletarUsuario(int pId)
        {
            var usuarioParaDeletar = UsuarioPorId(pId);

            _documentosBusiness.DeletarDocumento(pId);
            _enderecosBusiness.DeletarEndereco(pId);
            _telefonesBusiness.DeletarTelefone(usuarioParaDeletar);

            _usuariosCollection.DeleteOne(Builders<Usuarios>.Filter.Eq(usr=>usr.UsuarioId,pId));
        }
    }
}