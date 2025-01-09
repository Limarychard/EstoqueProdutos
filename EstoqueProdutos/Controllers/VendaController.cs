using EstoqueProdutos.Data;
using EstoqueProdutos.Enums;
using EstoqueProdutos.Filters;
using EstoqueProdutos.Helper;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        readonly private IConfiguracaoRepositorio _configuracaoRepositorio;


        public VendaController(
            IVendaRepositorio vendaRepositorio, 
            IClienteRepositorio clienteRepositorio,
            IProdutoRepositorio produtoRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            ISessao sessao,
            IConfiguracaoRepositorio configuracaoRepositorio
            )
        {
            _vendaRepositorio = vendaRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _configuracaoRepositorio = configuracaoRepositorio;

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

        public IActionResult Index(string? NomeCliente = null, int? Valor = null, FormaDePagamentoEnum? FormaDePagamento = null, bool? Parcelado = null, char? QtdParcela = null,  DateTime? DataVendaInicio = null, DateTime? DataVendaFim = null)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            ViewBag.FormasDePagamento = ObterFormasDePagamento();

            List<VendaModel> Vendas;
            if (usuarioLogado.Perfil == PerfilEnum.Admin)
            {
                Vendas = _vendaRepositorio.ListarTodos();
            }
            else
            {
                Vendas = _vendaRepositorio.ListarPorUsuarioId(usuarioLogado.Id);
            }

            if (!string.IsNullOrEmpty(NomeCliente))
                Vendas = Vendas.Where(u => u.Cliente.Nome.Contains(NomeCliente, StringComparison.OrdinalIgnoreCase)).ToList();

            if (Valor != null)
            {
                string valorFiltro = Valor.ToString();
                Vendas = Vendas.Where(u => u.Valor.ToString().Contains(valorFiltro)).ToList();
            }

            if (FormaDePagamento != null)
                Vendas = Vendas.Where(u => u.FormaDePagamento == FormaDePagamento).ToList();

            if (Parcelado.HasValue)
                Vendas = Vendas.Where(u => u.Parcelado == Parcelado.Value).ToList();

            if (QtdParcela != null)
            {
                string QtdParcelaFiltro = QtdParcela.ToString();
                Vendas = Vendas.Where(u => u.QuantidadeDeParcela.ToString().Contains(QtdParcelaFiltro)).ToList();
            }

            if (DataVendaInicio.HasValue)
                Vendas = Vendas.Where(u => u.DtInc >= DataVendaInicio.Value).ToList();

            if (DataVendaFim.HasValue)
                Vendas = Vendas.Where(u => u.DtInc <= DataVendaFim.Value).ToList();

            ViewData["NomeCliente"] = NomeCliente;
            ViewData["Valor"] = Valor;
            ViewData["FormaDePagamento"] = FormaDePagamento;
            ViewData["Parcelado"] = Parcelado;
            ViewData["QtdParcela"] = QtdParcela;
            ViewData["DataVendaInicio"] = DataVendaInicio?.ToString("yyyy-MM-dd");
            ViewData["DataVendaFim"] = DataVendaFim?.ToString("yyyy-MM-dd");


            return View(Vendas);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            var produtos = _produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id);
            var configuracaoDoUsuario = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);

            var displayField = configuracaoDoUsuario.PesquisarProduto == 1 ? "Codigo" : "Nome";

            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", displayField);
            ViewBag.ProdutosJson = JsonConvert.SerializeObject(produtos);

            ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
            ViewBag.FormasDePagamento = ObterFormasDePagamento();

            var vendaModel = new VendaModel
            {
                Valor = 0
            };

            return View(vendaModel);
        }

        [HttpPost]
        public IActionResult Criar(VendaModel venda, List<int> quantidadesProduto)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                var configuracaoDoUsuario = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);

                ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");

                var displayField = configuracaoDoUsuario.PesquisarProduto == 1 ? "Codigo" : "Nome";

                ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", displayField);

                ViewBag.FormasDePagamento = ObterFormasDePagamento();

                venda.Cliente = _clienteRepositorio.ListarPorId(venda.ClienteId);
                venda.UsuarioId = usuarioLogado.Id;

                ModelState.Remove("Cliente");
                ModelState.Remove("Produto");
                ModelState.Remove("Usuario");

                venda.Valor = 0;

                if (ModelState.IsValid)
                {

                    if (venda.QuantidadeDeParcela == null)
                    {
                        venda.QuantidadeDeParcela = 0;
                    }

                    List<ProdutoVendaModel> produtosVenda = new List<ProdutoVendaModel>();

                    for (int i = 0; i < venda.ProdutoVendaId.Count; i++)
                    {
                        var produto = _produtoRepositorio.ListarPorId(venda.ProdutoVendaId[i]);

                        if (produto == null || produto.Quantidade < quantidadesProduto[i])
                        {
                            TempData["MensagemErro"] = $"Estoque insuficiente para o produto {produto?.Nome}.";
                            return RedirectToAction("Index");
                        }

                        produto.Quantidade -= quantidadesProduto[i];
                        _produtoRepositorio.Alterar(produto);

                        produtosVenda.Add(new ProdutoVendaModel
                        {
                            ProdutoId = venda.ProdutoVendaId[i],
                            NomeProduto = produto.Nome,
                            Quantidade = quantidadesProduto[i],
                            ValorProduto = produto.ValorDeVenda
                        });


                        var valorTotal = produto.ValorDeVenda * quantidadesProduto[i];
                        venda.Valor += valorTotal;
                    }

                    _vendaRepositorio.Adicionar(venda, produtosVenda);

                    venda.Cliente.DtUltCompra = DateTime.Now;

                    _clienteRepositorio.Alterar(venda.Cliente);

                    TempData["MensagemSucesso"] = "Parabéns, você fez uma nova venda!";
                    return RedirectToAction("Index");
                }

                return View();

            } catch(Exception err)
            {
                var innerExceptionMessage = err.InnerException?.Message ?? "Sem detalhes adicionais";
                TempData["MensagemErro"] = $"Ops, não conseguimos criar sua nova venda. Detalhe do erro: {err.Message}. Detalhes internos: {innerExceptionMessage}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            var venda = _vendaRepositorio.ListarPorId(id);

            ViewBag.Clientes = new SelectList(
                _clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id),
                "Id",
                "Nome"
            );

            ViewBag.FormasDePagamento = ObterFormasDePagamento();

            return View(venda);
        }

        [HttpPost]
        public IActionResult Editar(VendaSemProdutoModel vendaSemProduto)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                ViewBag.Clientes = new SelectList(_clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
                ViewBag.FormasDePagamento = ObterFormasDePagamento();

                ModelState.Remove("Cliente");
                ModelState.Remove("Produto");
                ModelState.Remove("Usuario");

                VendaModel venda = null;

                if (ModelState.IsValid)
                {
                    venda = new VendaModel()
                    {
                        Id = vendaSemProduto.Id,
                        Valor = vendaSemProduto.Valor,
                        ClienteId = vendaSemProduto.ClienteId,
                        FormaDePagamento = vendaSemProduto.FormaDePagamento,
                        Parcelado = vendaSemProduto.Parcelado,
                        QuantidadeDeParcela = vendaSemProduto.QuantidadeDeParcela

                    };

                    _vendaRepositorio.Alterar(venda);
                    TempData["MensagemSucesso"] = "Venda editada com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(vendaSemProduto);
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar sua venda. Detalhes do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(VendaModel venda)
        {
            _vendaRepositorio.Deletar(venda);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CriarProdutoVenda(int vendaId, string NomeProduto)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            var configuracaoDoUsuario = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);

            var displayField = configuracaoDoUsuario.PesquisarProduto == 1 ? "Codigo" : "Nome";

            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", displayField)
                .Prepend(new SelectListItem { Text = "Selecione um produto", Value = "" });

            ViewBag.FormasDePagamento = ObterFormasDePagamento();

            ViewBag.VendaId = vendaId;
            ViewBag.NomeProduto = NomeProduto;

            return View();
        }

        [HttpPost]
        public IActionResult CriarProdutoVenda(ProdutoVendaModel produtoVenda, int vendaId)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                var configuracaoDoUsuario = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);

                var displayField = configuracaoDoUsuario.PesquisarProduto == 1 ? "Codigo" : "Nome";
                ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", displayField);

                ModelState.Remove("Venda");
                ModelState.Remove("Produto");

                if (ModelState.IsValid)
                {
                    var produto = _produtoRepositorio.ListarPorId(produtoVenda.ProdutoId);
                    var venda = _vendaRepositorio.ListarPorId(vendaId);

                    if (produto == null || produto.Quantidade < produto.Quantidade)
                    {
                        TempData["MensagemErro"] = $"Estoque insuficiente para o produto {produtoVenda.NomeProduto}.";
                        return RedirectToAction("Index");
                    }

                    // Alterando a quantidade de produto no ProdutoModel
                    produto.Quantidade = -produtoVenda.Quantidade;
                    _produtoRepositorio.Alterar(produto);

                    // Alterando o valor total da minha venda
                    var valorTotal = produtoVenda.ValorProduto * produtoVenda.Quantidade;
                    venda.Valor += valorTotal;
                    _vendaRepositorio.Alterar(venda);


                    // Adicionando o produto
                    _vendaRepositorio.AdicionarProdutoVenda(produtoVenda);

                    TempData["MensagemSucesso"] = $"O {produtoVenda.NomeProduto} foi adicionado a venda!";
                }

                return RedirectToAction("Index");
            } 
            catch(Exception err){
                TempData["MensagemErro"] = $"Ops, não conseguimos adicionar o Produto à sua venda. Detalhes do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult EditarProdutoVenda(int vendaId, int id)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
            ViewBag.VendaId = vendaId;

            var produtoVenda = _vendaRepositorio.ListarProdutoVendaPorId(id);

            if (produtoVenda == null || produtoVenda.VendaId != vendaId)
            {
                return NotFound();
            }

            ViewBag.FormasDePagamento = ObterFormasDePagamento();
            return View(produtoVenda);
        }

        [HttpPost]
        public IActionResult EditarProdutoVenda(int vendaId, ProdutoVendaModel produtoVenda)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                var produtoVendaAtual = _vendaRepositorio.ListarProdutoVendaPorId(produtoVenda.Id);

                if (produtoVendaAtual == null)
                {
                    TempData["MensagemErro"] = "Produto da venda não encontrado.";
                    return RedirectToAction("Detalhes", new { id = vendaId });
                }

                var produto = _produtoRepositorio.ListarPorId(produtoVenda.ProdutoId);

                if (produto == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado no estoque.";
                    return RedirectToAction("Detalhes", new { id = vendaId });
                }

                produtoVenda.NomeProduto = produto.Nome;

                var diferencaQuantidade = produtoVenda.Quantidade - produtoVendaAtual.Quantidade;

                if (produto.Quantidade < diferencaQuantidade)
                {
                    TempData["MensagemErro"] = $"Estoque insuficiente para o produto {produto.Nome}.";
                    return RedirectToAction("Detalhes", new { id = vendaId });
                }

                produto.Quantidade -= diferencaQuantidade;
                _produtoRepositorio.Alterar(produto);

                produtoVendaAtual.NomeProduto = produto.Nome;
                produtoVendaAtual.Quantidade = produtoVenda.Quantidade;
                produtoVendaAtual.ValorProduto = produtoVenda.ValorProduto;

                _vendaRepositorio.AlterarProdutoVenda(produtoVendaAtual);

                var venda = _vendaRepositorio.ListarPorId(vendaId);

                if (venda != null && venda.ProdutoVenda != null)
                {
                    venda.Valor = 0;

                    Console.WriteLine($"Total de produtos na venda: {venda.ProdutoVenda.Count}");

                    foreach (var itemProdutoVenda in venda.ProdutoVenda)
                    {
                        Console.WriteLine($"Processando produto: {itemProdutoVenda.NomeProduto}, Quantidade: {itemProdutoVenda.Quantidade}, Valor: {itemProdutoVenda.ValorProduto}");

                        var valorTotalProduto = itemProdutoVenda.ValorProduto * itemProdutoVenda.Quantidade;
                        venda.Valor += valorTotalProduto;
                    }
                    _vendaRepositorio.Alterar(venda);
                }

                TempData["MensagemSucesso"] = "Produto editado e valor total atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                var innerExceptionMessage = err.InnerException?.Message ?? "Sem detalhes adicionais";
                TempData["MensagemErro"] = $"Ops, não conseguimos editar o produto da venda. Detalhes do erro: {err.Message}. Detalhes internos: {innerExceptionMessage}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ApagarProduto(ProdutoVendaModel produtoVenda)
        {
            _vendaRepositorio.DeletarProduto(produtoVenda);

            var venda = _vendaRepositorio.ListarPorId(produtoVenda.VendaId);

            if (venda != null && venda.ProdutoVenda != null)
            {
                venda.Valor = 0;

                Console.WriteLine($"Total de produtos na venda: {venda.ProdutoVenda.Count}");

                foreach (var itemProdutoVenda in venda.ProdutoVenda)
                {
                    Console.WriteLine($"Processando produto: {itemProdutoVenda.NomeProduto}, Quantidade: {itemProdutoVenda.Quantidade}, Valor: {itemProdutoVenda.ValorProduto}");

                    var valorTotalProduto = itemProdutoVenda.ValorProduto * itemProdutoVenda.Quantidade;
                    venda.Valor += valorTotalProduto;
                }
                _vendaRepositorio.Alterar(venda);
            }

            return RedirectToAction("Index");
        }
    }
}
