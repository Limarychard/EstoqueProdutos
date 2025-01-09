using System.Text.RegularExpressions;

namespace EstoqueProdutos.Utils
{
    public class FormataUtils
    {
        public static string FormatCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return string.Empty;
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        public static string FormatCNPJ(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj)) return string.Empty;
            return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string ValidaCPFouCNPJ(string documento)
        {
            if (string.IsNullOrEmpty(documento)) return "Inválido";

            documento = new string(documento.Where(char.IsDigit).ToArray());

            if (documento.Length == 11)
                return "CPF";
            else if (documento.Length == 14)
                return "CNPJ";

            return "Inválido";
        }

        public static string FormataCPFouCNPJ(long? documento)
        {
            if (string.IsNullOrEmpty(documento.ToString())) return "CPF/CNPJ não cadastrado";

            var tipo = ValidaCPFouCNPJ(documento.ToString());

            if (tipo == "CPF")
                return Convert.ToUInt64(documento.ToString()).ToString(@"000\.000\.000\-00");
            else if (tipo == "CNPJ")
                return Convert.ToUInt64(documento.ToString()).ToString(@"00\.000\.000\/0000\-00");

            return "Documento inválido";
        }

        public static string FormatTelefone(long telefone)
        {
            if (string.IsNullOrEmpty(telefone.ToString())) return string.Empty;
            var mask = telefone.ToString().Length > 10 ? @"(00) 00000\-0000" : @"(00) 0000\-0000";
            return Convert.ToUInt64(telefone).ToString(mask);
        }

        public static string FormatCEP(string cep)
        {
            if (string.IsNullOrEmpty(cep)) return string.Empty;
            return Convert.ToUInt64(cep).ToString(@"00000\-000");
        }

        public static string FormatValor(decimal? valor)
        {
            if(!valor.HasValue)
            {
                valor = 0;
                return valor?.ToString("C");
            }

            return valor?.ToString("C"); // Formato monetário com base na cultura local
        }

        public static string RemoverCaracterEspecial(string input)
        {
            // Expressão regular para manter apenas letras, números e espaços
            return Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");
        }
    }
}
