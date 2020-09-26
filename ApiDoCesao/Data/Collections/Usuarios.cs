using System.Collections.Generic;

namespace ApiDoCesao.Data.Collections
{
    public class Usuarios
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get;set; }
        public string Senha { get; set; }
        public Documentos Documento { get; set; }
        public Enderecos Endereco { get; set; }
        public Telefones Telefone { get; set; }

        public ICollection<Produtos> Produtos { get; set; } = new List<Produtos>();

        public Usuarios(
            string nome,
            string sobrenome,
            string email,
            string senha
          )
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Email = email;
            Senha = senha;
        }

        public void AddProduto(Produtos pProduto)
        {
            Produtos.Add(pProduto);
        }

        public void RemoveProduto(Produtos pProduto)
        {
            Produtos.Remove(pProduto);
        }
    }
}
