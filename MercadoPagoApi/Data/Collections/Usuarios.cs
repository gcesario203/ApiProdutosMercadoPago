using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MercadoPagoApi.Data.Collections
{
    public class Usuarios
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get;set; }
        public string Senha { get; set; }
        public string Funcao { get; set; }
        public Documentos Documento { get; set; }
        public Enderecos Endereco { get; set; }
        public Telefones Telefone { get; set; }
        public List<Produtos> CarrinhoDeCompras { get; set; } = new List<Produtos>();

        public Usuarios(
            string nome,
            string sobrenome,
            string email,
            string senha,
            string funcao
          )
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Email = email;
            Senha = senha;
            Funcao = funcao;
        }
    }
}
