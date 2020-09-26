using ApiDoCesao.Enums;
using MongoDB.Bson.Serialization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ApiDoCesao.General
{
    public static class Utils
    {
        public static void JsonMapClasses<T>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(item =>
                {
                    item.AutoMap();
                    item.SetIgnoreExtraElements(true);
                });
            }
        }

        public static bool EmailValido(string pEmail)
        {
            try 
            {
                var validMail = new MailAddress(pEmail);

                return true;
            } 
            catch 
            {
                return false;
            }
        }

        public static bool SenhaValida(string pPassword)
        {
            if (pPassword.Length>=6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DocumentoValido(string pDoc, string pTipo)
        {
            if (pTipo == TipoDocumento.CPF)
            {
                return pDoc.Length == 11 ? true : false;
            }
            else if(pTipo == TipoDocumento.CNPJ)
            {
                return pDoc.Length == 14 ? true : false;
            }
            else
            {
                return false;
            }
        }

        public static bool CepValido(string pCep)
        {
            return pCep.Length == 8 ? true : false;
        }

        public static bool TelValido(string pArea, string pNumero)
        {
            return (pArea.Length == 3 && pNumero.Length == 8) ? true : false;
        }
    }
}
