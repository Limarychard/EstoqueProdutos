using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface IUsuarioRepositorio
    {
        UsuarioModel BuscarPorLogin(string login);
        UsuarioModel BuscarPorEmailELogin(string login, string email);
        UsuarioModel ListarPorId(int id);
        List<UsuarioModel> ListarTodos();
        UsuarioModel Adicionar(UsuarioModel usuario);
        UsuarioModel Alterar(UsuarioModel usuario);
        UsuarioModel Deletar(UsuarioModel usuario);
        UsuarioModel AlterarSenha(AlterarSenhaModel usuario);
    }
}
