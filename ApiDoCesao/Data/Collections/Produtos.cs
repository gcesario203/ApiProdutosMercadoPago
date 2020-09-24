using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDoCesao.Data.Collections
{
    public class Produtos
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }

        public Produtos(int pProdutoId,string pNome, double pPreco = 0.0)
        {
            ProdutoId = pProdutoId;
            Nome = pNome;
            Preco = pPreco;
        }
    }
}
