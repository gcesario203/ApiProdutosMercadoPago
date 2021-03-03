using System;
using System.Collections.Generic;
using System.Linq;
using ApiDoCesao.Data.Collections;
using ApiDoCesao.General;
using MercadoPago;
using MercadoPago.Resources;

namespace ApiDoCesao.Data.Business
{
    public class ComprasBusiness
    {
        private UsuariosBusiness _usuariosCollection { get; set; }
        private ProdutosBusiness _produtosCollection { get; set; }

        public ComprasBusiness(MongoDb dbContext)
        {
            _usuariosCollection = new UsuariosBusiness(dbContext);
            _produtosCollection = new ProdutosBusiness(dbContext);
        }

        public void AdicionarProdutoAoCarrinho(int pId, int pQuantidade,string pEmailAutenticado)
        {
            try
            {
                var Produto = _produtosCollection.ProdutoPorId(pId);
                var Usuario = _usuariosCollection.UsuarioPorEmail(pEmailAutenticado);
                var ProdutoDoCarrinho = Usuario.CarrinhoDeCompras.FirstOrDefault(prod => prod.ProdutoId == Produto.ProdutoId);
                var ProdutosDobanco = Produto.Quantidade - pQuantidade;

                if (ProdutosDobanco < 0)
                {
                    throw new Exception("Quantidade indisponível");
                }
                if( ProdutoDoCarrinho== null)
                {
                    Produto.Quantidade = pQuantidade;
                    Usuario.CarrinhoDeCompras.Add(Produto);
                    _usuariosCollection.AtualizarCarrinho(Usuario);
                    Produto.Quantidade = ProdutosDobanco;

                    _produtosCollection.AlterarQuantidade(Produto);
                    
                }
                else
                {
                    ProdutoDoCarrinho.Quantidade += pQuantidade;
                    Produto.Quantidade -= pQuantidade;

                    _produtosCollection.AlterarQuantidade(Produto);
                    _usuariosCollection.AtualizarCarrinho(Usuario);
                }
            }
            catch
            {
                throw new Exception("Não foi possivel adicionar este produto ao carrinho");
            }
            
        }

        public void RemoverProdutoDoCarrinho(int pId, int pQuantidade, string pEmailAutenticado)
        {
            try
            {
                var Produto = _produtosCollection.ProdutoPorId(pId);
                var Usuario = _usuariosCollection.UsuarioPorEmail(pEmailAutenticado);
                var ProdutoDoCarrinho = Usuario.CarrinhoDeCompras.FirstOrDefault(prod=>prod.ProdutoId == Produto.ProdutoId);

                if (ProdutoDoCarrinho != null && ProdutoDoCarrinho.Quantidade - pQuantidade >=  0)
                {
                    ProdutoDoCarrinho.Quantidade -= pQuantidade;
                    Produto.Quantidade += pQuantidade;

                    if(ProdutoDoCarrinho.Quantidade > 0)
                    {

                        _produtosCollection.AlterarQuantidade(Produto);
                        _usuariosCollection.AtualizarCarrinho(Usuario);
                    }
                    else
                    {
                        Usuario.CarrinhoDeCompras.Remove(ProdutoDoCarrinho);
                        
                        _produtosCollection.AlterarQuantidade(Produto);
                        _usuariosCollection.AtualizarCarrinho(Usuario);
                    }

                }
                else
                {
                    throw new Exception("Produto não se encontra no carrinho");
                }

                
            }
            catch
            {
                throw new Exception("Produto não se encontra no carrinho");
            }
        }

        public dynamic ValidarCompra(string pEmailAutenticado)
        {
            var lUser = _usuariosCollection.UsuarioPorEmail(pEmailAutenticado);

            if(lUser == null)
            {
                throw new Exception("Usuário inválido");
            }

            try
            {
                var Preference = new Preference()
                {
                    Payer = Utils.MercadoPagoPagante(lUser),
                };

                foreach (var Produto in lUser.CarrinhoDeCompras)
                {
                    Preference.Items.Add(Utils.MercadoPagoProduto(Produto));
                }

                Preference.Save();

                return Preference.InitPoint;
            }
            catch(Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}
