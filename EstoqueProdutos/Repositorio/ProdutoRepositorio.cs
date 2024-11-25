using EstoqueProdutos.Data;
using EstoqueProdutos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EstoqueProdutos.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly BancoContext _bancoContext;
        public ProdutoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public ProdutoModel ListarPorId(int id)
        {
            return _bancoContext.Produtos.FirstOrDefault(x => x.Id == id);
        }

        public List<ProdutoModel> ListarPorUsuarioId(int usuarioId)
        {
            return _bancoContext.Produtos
                .Include(v => v.Usuario)
                .Where(v => v.UsuarioId == usuarioId)
                .ToList();
        }

        public List<ProdutoModel> ListarTodos()
        {
            return _bancoContext.Produtos.ToList();
        }

        public ProdutoModel ObterPorId(int id)
        {
            return _bancoContext.Produtos.Find(id);
        }

        public ProdutoModel Adicionar(ProdutoModel produto)
        {
            _bancoContext.Produtos.Add(produto);
            _bancoContext.SaveChanges();
            return produto;
        }

        public ProdutoModel Alterar(ProdutoModel produto)
        {
            var produtoExistente = _bancoContext.Produtos.FirstOrDefault(p => p.Id == produto.Id);

            if (produtoExistente == null)
            {
                throw new Exception("Produto não encontrado no banco de dados.");
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Valor = produto.Valor;
            produtoExistente.Quantidade = produto.Quantidade;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.Foto = produto.Foto;

            _bancoContext.SaveChanges();

            return produtoExistente;
        }

        public ProdutoModel Deletar(ProdutoModel produto)
        {
            _bancoContext.Remove(produto);
            _bancoContext.SaveChanges();
            return produto;
        }

        public PagedResult<ProdutoModel> GetPaged(int page, int pageSize)
        {
            var totalItems = _bancoContext.Produtos.Count();

            var produtos = _bancoContext.Produtos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<ProdutoModel>
            {
                Items = produtos,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public int Count()
        {
            return _bancoContext.Produtos.Count();
        }
    }
}
