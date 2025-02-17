﻿using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface IUsuarioRepositorio
    {
        UsuarioModel BuscarPorLogin(string login);
        UsuarioModel BuscarPorEmailELogin(string login, string email);
        UsuarioModel ObterPorId(int id);
        UsuarioModel ListarPorId(int id);
        List<UsuarioModel> ListarTodos();
        UsuarioModel Adicionar(UsuarioModel usuario);
        UsuarioModel Alterar(UsuarioModel usuario);
        UsuarioModel AlterarSenha(AlterarSenhaModel usuario);
        UsuarioModel AlterarEmail(AlterarEmailModel usuario);
        UsuarioModel Deletar(UsuarioModel usuario);
    }
}
