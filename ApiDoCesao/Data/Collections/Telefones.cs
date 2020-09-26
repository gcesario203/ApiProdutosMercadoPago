using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDoCesao.Data.Collections
{
    public class Telefones
    {
        public int TelefoneId { get; set; }
        public int UsuarioId { get; set; }
        public string Area { get; set; }
        public string Numero { get; set; }

        public Telefones( string area, string numero)
        {
            Area = area;
            Numero = numero;
        }
    }
}
