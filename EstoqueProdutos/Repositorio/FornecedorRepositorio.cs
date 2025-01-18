using EstoqueProdutos.Data;
using EstoqueProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueProdutos.Repositorio
{
    public class FornecedorRepositorio : IFornecedorRepositorio
    {
        private readonly BancoContext _bancoContext;
        public FornecedorRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public FornecedorModel ListarPorId(int id)
        {
            return _bancoContext.Fornecedores.FirstOrDefault(x => x.Id == id);
        }

        public List<FornecedorModel> ListarPorUsuarioId(int usuarioId)
        {
            return _bancoContext.Fornecedores
                .Where(v => v.UsuarioId == usuarioId)
                .ToList();
        }

        public List<FornecedorModel> ListarTodos()
        {
            return _bancoContext.Fornecedores.ToList();
        }

        public FornecedorModel Adicionar(FornecedorModel fornecedor)
        {
            _bancoContext.Fornecedores.Add(fornecedor);
            _bancoContext.SaveChanges();
            return fornecedor;
        }

        public FornecedorModel Alterar(FornecedorModel fornecedor)
        {
            _bancoContext.Update(fornecedor);
            _bancoContext.SaveChanges();
            return fornecedor;
        }

        public FornecedorModel Deletar(FornecedorModel fornecedor)
        {
            _bancoContext.Remove(fornecedor);
            _bancoContext.SaveChanges();
            return fornecedor;
        }

    }
}
