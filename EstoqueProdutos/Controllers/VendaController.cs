using EstoqueProdutos.Data;
using EstoqueProdutos.Enums;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EstoqueProdutos.Controllers
{
    public class VendaController : Controller
    {
        readonly private IVendaRepositorio _vendaRepositorio;
        readonly private IClienteRepositorio _clienteRepositorio;
        readonly private IProdutoRepositorio _produtoRepositorio;


        public VendaController(
            IVendaRepositorio vendaRepositorio, 
            IClienteRepositorio clienteRepositorio,
            IProdutoRepositorio produtoRepositorio
            )
        {
            _vendaRepositorio = vendaRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _produtoRepositorio = produtoRepositorio;
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
            var Vendas = _vendaRepositorio.ListarTodos();
            return View(Vendas);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarTodos(), "Id", "Nome");
            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarTodos(), "Id", "Nome");
            ViewBag.FormasDePagamento = ObterFormasDePagamento();
            return View();
        }

        [HttpPost]
        public IActionResult Criar(VendaModel venda)
        {
            try
            {
                ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarTodos(), "Id", "Nome");
                ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarTodos(), "Id", "Nome");
                ViewBag.FormasDePagamento = ObterFormasDePagamento();

                venda.Cliente = _clienteRepositorio.ListarPorId(venda.ClienteId);
                venda.Produto = _produtoRepositorio.ListarPorId(venda.ProdutoId);

                ModelState.Remove("Cliente");
                ModelState.Remove("Produto");

                if (ModelState.IsValid)
                {
                    if (venda.QuantidadeDeParcela == null)
                    {
                        venda.QuantidadeDeParcela = 0;
                    }
                    _vendaRepositorio.Adicionar(venda);


                    var produto = _produtoRepositorio.ListarPorId(venda.ProdutoId);

                    if(produto != null && produto.Quantidade >= venda.QuantidadeProduto)
                    {
                        produto.Quantidade -= venda.QuantidadeProduto;
                        _produtoRepositorio.Alterar(produto);
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Estoque insuficiente para a venda.";
                        return RedirectToAction("Index");
                    }

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
            VendaModel venda = _vendaRepositorio.ListarPorId(id);
            ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarTodos(), "Id", "Nome");
            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarTodos(), "Id", "Nome");
            ViewBag.FormasDePagamento = ObterFormasDePagamento();
            return View(venda);
        }

        [HttpPost]
        public IActionResult Editar(VendaModel venda)
        {
            try
            {
                ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarTodos(), "Id", "Nome");
                ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarTodos(), "Id", "Nome");
                ViewBag.FormasDePagamento = ObterFormasDePagamento();

                ModelState.Remove("Cliente");
                ModelState.Remove("Produto");

                if (ModelState.IsValid)
                {
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