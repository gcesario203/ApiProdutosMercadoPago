using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDoCesao.Models
{
    public class UsuarioDto
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public AuthDto Login { get; set; }
        public string SenhaConfirmacao { get; set; }
        public TelefoneDto Telefone { get; set; }
        public DocumentoDto Documento { get; set; }
        public EnderecoDto Endereco { get; set; }
    }
}
