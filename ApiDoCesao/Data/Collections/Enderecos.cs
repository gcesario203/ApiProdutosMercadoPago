using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDoCesao.Data.Collections
{
    public class Enderecos
    {
        public int EnderecoId { get; set; }
        public int UsuarioId { get; set; }
        public int Numero { get; set; }
        public string Rua { get; set; }
        public string Cep { get; set; }

        public Enderecos(int pNumero, string pRua, string pCep)
        {
            Numero = pNumero;
            Rua = pRua;
            Cep = pCep;
        }
    }
}
