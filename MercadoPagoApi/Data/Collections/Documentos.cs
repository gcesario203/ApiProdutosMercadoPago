namespace MercadoPagoApi.Data.Collections
{
    public class Documentos
    {
        public int DocumentoId { get; set; }
        public int UsuarioId { get; set; }
        public string Tipo { get; set; }
        public string Numero { get; set; }

        public Documentos(string pTipo, string pNumero)
        {
            Tipo = pTipo;
            Numero = pNumero;
        }
    }
}
