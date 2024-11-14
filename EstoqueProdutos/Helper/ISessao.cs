using EstoqueProdutos.Models;

namespace EstoqueProdutos.Helper
{
    public interface ISessao
    {
        void CriarSessaoUsuaroio(UsuarioModel usuario);
        void RemoverSessaoUsuaroio();
        UsuarioModel BuscarSessaoDoUsuario();
    }
}
