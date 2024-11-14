using EstoqueProdutos.Models;
using System.Security.Cryptography;
using System.Text;

namespace EstoqueProdutos.Helper
{
    public static class Criptografia
    {
        public static string GerarHash(this string senha)
        {
            var hash = SHA1.Create();
            var encoding = new ASCIIEncoding();
            var array = encoding.GetBytes(senha);

            array = hash.ComputeHash(array);

            var strHexa = new StringBuilder();

            foreach (var i in array)
            {
                strHexa.Append(i.ToString("x2"));
            }

            return strHexa.ToString();
        }
    }
}
