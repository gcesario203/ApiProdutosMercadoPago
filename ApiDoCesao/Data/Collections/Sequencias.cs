using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDoCesao.Data.Collections
{
    public class Sequencias
    {
        public int Atual { get; set; }
        public string Nome { get; set; }

        public Sequencias(int pAtual, string pNome)
        {
            Atual = pAtual;
            Nome = pNome;
        }
    }
}
