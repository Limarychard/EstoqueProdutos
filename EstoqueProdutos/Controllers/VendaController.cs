using EstoqueProdutos.Data;
using EstoqueProdutos.Enums;
using EstoqueProdutos.Filters;
using EstoqueProdutos.Helper;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EstoqueProdutos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class VendaController : Controller
    {
        readonly private IVendaRepositorio _vendaRepositorio;
        readonly private IClienteRepositorio _clienteRepositorio;
        readonly private IProdutoRepositorio _produtoRepositorio;
        readonly private IUsuarioRepositorio _usuarioRepositorio;
        readonly private ISessao _sessao;


        public VendaController(
            IVendaRepositorio vendaRepositorio, 
            IClienteRepositorio clienteRepositorio,
            IProdutoRepositorio produtoRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            ISessao sessao
            )
        {
            _vendaRepositorio = vendaRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;

        }

        private List<SelectListItem> ObterFormasDePagamento()
        {
            return Enum.GetValues(typeof(FormaDePagamentoEnum))
                .Cast<FormaDePagamentoEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = FormaDePagamentoDisplay(e)
                }).ToList();
        }

        private string FormaDePagamentoDisplay(FormaDePagamentoEnum formaDePagamento)
        {
            var displayAttribute = formaDePagamento
                .GetType()
                .GetField(formaDePagamento.ToString())
                ?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? formaDePagamento.ToString();
        }

        public IActionResult Index()
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            List<VendaModel> Vendas;
            if (usuarioLogado.Perfil == PerfilEnum.Admin)
            {
                Vendas = _vendaRepositorio.ListarTodos();
            }
            else
            {
                Vendas = _vendaRepositorio.ListarPorUsuarioId(usuarioLogado.Id);
            }

            return View(Vendas);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
            ViewBag.FormasDePagamento = ObterFormasDePagamento();
            return View();
        }

        [HttpPost]
        public IActionResult Criar(VendaModel venda)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
                ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
                ViewBag.FormasDePagamento = ObterFormasDePagamento();

                venda.Cliente = _clienteRepositorio.ListarPorId(venda.ClienteId);
                venda.Produto = _produtoRepositorio.ListarPorId(venda.ProdutoId);

                ModelState.Remove("Cliente");
                ModelState.Remove("Produto");
                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {
                    if (venda.QuantidadeDeParcela == null)
                    {
                        venda.QuantidadeDeParcela = 0;
                    }

                    venda.UsuarioId = usuarioLogado.Id;

                    var produto = _produtoRepositorio.ListarPorId(venda.ProdutoId);

                    if(!(produto != null && produto.Quantidade >= venda.QuantidadeProduto))
                    {
                        TempData["MensagemErro"] = "Estoque insuficiente para a venda.";
                        return RedirectToAction("Index");
                    }

                    produto.Quantidade -= venda.QuantidadeProduto;
                    _produtoRepositorio.Alterar(produto);
                    _vendaRepositorio.Adicionar(venda);

                    TempData["MensagemSucesso"] = "Parabéns, você fez uma nova venda!";
                    return RedirectToAction("Index");
                }
                return View();
            } catch(Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos criar sua nova venda, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            VendaModel venda = _vendaRepositorio.ListarPorId(id);
            ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
            ViewBag.FormasDePagamento = ObterFormasDePagamento();
            return View(venda);
        }

        [HttpPost]
        public IActionResult Editar(VendaModel venda)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
                ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
                ViewBag.FormasDePagamento = ObterFormasDePagamento();

                ModelState.Remove("Cliente");
                ModelState.Remove("Produto");
                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {
                    venda.UsuarioId = usuarioLogado.Id;

                    var produto = _produtoRepositorio.ListarPorId(venda.ProdutoId);

                    if (!(produto != null && produto.Quantidade >= venda.QuantidadeProduto))
                    {
                        TempData["MensagemErro"] = "Estoque insuficiente para a venda.";
                        return RedirectToAction("Index");
                    }

                    produto.Quantidade -= venda.QuantidadeProduto;
                    _produtoRepositorio.Alterar(produto);
                    _vendaRepositorio.Alterar(venda);
                    TempData["MensagemSucesso"] = "Venda editada com sucesso!";
                    return RedirectToAction("Index");
                }

                return View();
            } catch(Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar sua venda, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(VendaModel venda)
        {
            _vendaRepositorio.Deletar(venda);
            return RedirectToAction("Index");
        }
    }
}