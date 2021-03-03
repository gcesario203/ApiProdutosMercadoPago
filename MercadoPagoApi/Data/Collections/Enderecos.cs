using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercadoPagoApi.Data.Collections
{
    public class Enderecos
    {
        public int EnderecoId { get; set; }
        public int UsuarioId { get; set; }
        public string Numero { get; set; }
        public string Rua { get; set; }
        public string Cep { get; set; }

        public Enderecos(string pNumero, string pRua, string pCep)
        {
            Numero = pNumero;
            Rua = pRua;
            Cep = pCep;
        }
    }
}
