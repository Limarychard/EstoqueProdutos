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
    public class CompraController : Controller
    {
        readonly private ICompraRepositorio _compraRepositorio;
        readonly private IFornecedorRepositorio _fornecedorRepositorio;
        readonly private IProdutoRepositorio _produtoRepositorio;
        readonly private IUsuarioRepositorio _usuarioRepositorio;
        readonly private ISessao _sessao;
        readonly private IConfiguracaoRepositorio _configuracaoRepositorio;


        public CompraController(
            ICompraRepositorio compraRepositorio, 
            IFornecedorRepositorio fornecedorRepositorio,
            IProdutoRepositorio produtoRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            ISessao sessao,
            IConfiguracaoRepositorio configuracaoRepositorio
            )
        {
            _compraRepositorio = compraRepositorio;
            _fornecedorRepositorio = fornecedorRepositorio;
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

        public IActionResult Index(string? NomeFornecedor = null, int? Valor = null, FormaDePagamentoEnum? FormaDePagamento = null, bool? Parcelado = null, char? QtdParcela = null,  DateTime? DataCompraInicio = null, DateTime? DataCompraFim = null)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            ViewBag.FormasDePagamento = ObterFormasDePagamento();

            List<CompraModel> Compras;
            if (usuarioLogado.Perfil == PerfilEnum.Admin)
            {
                Compras = _compraRepositorio.ListarTodos();
            }
            else
            {
                Compras = _compraRepositorio.ListarPorUsuarioId(usuarioLogado.Id);
            }

            if (!string.IsNullOrEmpty(NomeFornecedor))
                Compras = Compras.Where(u => u.Fornecedor.Nome.Contains(NomeFornecedor, StringComparison.OrdinalIgnoreCase)).ToList();

            if (Valor != null)
            {
                string valorFiltro = Valor.ToString();
                Compras = Compras.Where(u => u.Valor.ToString().Contains(valorFiltro)).ToList();
            }

            if (FormaDePagamento != null)
                Compras = Compras.Where(u => u.FormaDePagamento == FormaDePagamento).ToList();

            if (Parcelado.HasValue)
                Compras = Compras.Where(u => u.Parcelado == Parcelado.Value).ToList();

            if (QtdParcela != null)
            {
                string QtdParcelaFiltro = QtdParcela.ToString();
                Compras = Compras.Where(u => u.QuantidadeDeParcela.ToString().Contains(QtdParcelaFiltro)).ToList();
            }

            if (DataCompraInicio.HasValue)
                Compras = Compras.Where(u => u.DtInc >= DataCompraInicio.Value).ToList();

            if (DataCompraFim.HasValue)
                Compras = Compras.Where(u => u.DtInc <= DataCompraFim.Value).ToList();

            ViewData["NomeFornecedor"] = NomeFornecedor;
            ViewData["Valor"] = Valor;
            ViewData["FormaDePagamento"] = FormaDePagamento;
            ViewData["Parcelado"] = Parcelado;
            ViewData["QtdParcela"] = QtdParcela;
            ViewData["DataCompraInicio"] = DataCompraInicio?.ToString("yyyy-MM-dd");
            ViewData["DataCompraFim"] = DataCompraFim?.ToString("yyyy-MM-dd");


            return View(Compras);
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

            ViewBag.Fornecedores = new SelectList(_fornecedorRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
            ViewBag.FormasDePagamento = ObterFormasDePagamento();

            var compraModel = new CompraModel
            {
                Valor = 0
            };

            return View(compraModel);
        }

        [HttpPost]
        public IActionResult Criar(CompraModel compra, List<int> quantidadesProduto)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                var configuracaoDoUsuario = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);

                ViewBag.Fornecedores = new SelectList(_fornecedorRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");

                var displayField = configuracaoDoUsuario.PesquisarProduto == 1 ? "Codigo" : "Nome";

                ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", displayField);

                ViewBag.FormasDePagamento = ObterFormasDePagamento();

                compra.Fornecedor = _fornecedorRepositorio.ListarPorId(compra.FornecedorId);
                compra.UsuarioId = usuarioLogado.Id;

                ModelState.Remove("Fornecedor");
                ModelState.Remove("Produto");
                ModelState.Remove("Usuario");

                compra.Valor = 0;

                if (ModelState.IsValid)
                {

                    if (compra.QuantidadeDeParcela == null)
                    {
                        compra.QuantidadeDeParcela = 0;
                    }

                    List<ProdutoCompraModel> produtosCompra = new List<ProdutoCompraModel>();

                    for (int i = 0; i < compra.ProdutoCompraId.Count; i++)
                    {
                        var produto = _produtoRepositorio.ListarPorId(compra.ProdutoCompraId[i]);

                        produto.Quantidade += quantidadesProduto[i];
                        _produtoRepositorio.Alterar(produto);

                        produtosCompra.Add(new ProdutoCompraModel
                        {
                            ProdutoId = compra.ProdutoCompraId[i],
                            NomeProduto = produto.Nome,
                            Quantidade = quantidadesProduto[i],
                            ValorProduto = produto.ValorDeCusto
                        });


                        var valorTotal = produto.ValorDeCusto * quantidadesProduto[i];
                        compra.Valor += valorTotal;
                    }

                    _compraRepositorio.Adicionar(compra, produtosCompra);

                    compra.Fornecedor.DtUltCompra = DateTime.Now;

                    _fornecedorRepositorio.Alterar(compra.Fornecedor);

                    TempData["MensagemSucesso"] = "Parabéns, você fez uma nova compra!";
                    return RedirectToAction("Index");
                }

                return View();

            } catch(Exception err)
            {
                var innerExceptionMessage = err.InnerException?.Message ?? "Sem detalhes adicionais";
                TempData["MensagemErro"] = $"Ops, não conseguimos criar sua nova compra. Detalhe do erro: {err.Message}. Detalhes internos: {innerExceptionMessage}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            var compra = _compraRepositorio.ListarPorId(id);

            ViewBag.Fornecedores = new SelectList(
                _fornecedorRepositorio.ListarPorUsuarioId(usuarioLogado.Id),
                "Id",
                "Nome"
            );

            ViewBag.FormasDePagamento = ObterFormasDePagamento();

            return View(compra);
        }

        [HttpPost]
        public IActionResult Editar(CompraSemProdutoModel compraSemProduto)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                ViewBag.Fornecedores = new SelectList(_fornecedorRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
                ViewBag.FormasDePagamento = ObterFormasDePagamento();

                ModelState.Remove("Fornecedor");
                ModelState.Remove("Produto");
                ModelState.Remove("Usuario");

                CompraModel compra = null;

                if (ModelState.IsValid)
                {
                    compra = new CompraModel()
                    {
                        Id = compraSemProduto.Id,
                        Valor = compraSemProduto.Valor,
                        FornecedorId = compraSemProduto.FornecedorId,
                        FormaDePagamento = compraSemProduto.FormaDePagamento,
                        Parcelado = compraSemProduto.Parcelado,
                        QuantidadeDeParcela = compraSemProduto.QuantidadeDeParcela

                    };

                    _compraRepositorio.Alterar(compra);
                    TempData["MensagemSucesso"] = "Compra editada com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(compraSemProduto);
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar sua compra. Detalhes do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(CompraModel compra)
        {
            _compraRepositorio.Deletar(compra);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CriarProdutoCompra(int compraId, string NomeProduto)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            var configuracaoDoUsuario = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);

            var displayField = configuracaoDoUsuario.PesquisarProduto == 1 ? "Codigo" : "Nome";

            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", displayField)
                .Prepend(new SelectListItem { Text = "Selecione um produto", Value = "" });

            ViewBag.FormasDePagamento = ObterFormasDePagamento();

            ViewBag.CompraId = compraId;
            ViewBag.NomeProduto = NomeProduto;

            return View();
        }

        [HttpPost]
        public IActionResult CriarProdutoCompra(ProdutoCompraModel produtoCompra, int compraId)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                var configuracaoDoUsuario = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);

                var displayField = configuracaoDoUsuario.PesquisarProduto == 1 ? "Codigo" : "Nome";
                ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", displayField);

                ModelState.Remove("Compra");
                ModelState.Remove("Produto");

                if (ModelState.IsValid)
                {
                    var produto = _produtoRepositorio.ListarPorId(produtoCompra.ProdutoId);
                    var compra = _compraRepositorio.ListarPorId(compraId);

                    if (produto == null || produto.Quantidade < produto.Quantidade)
                    {
                        TempData["MensagemErro"] = $"Estoque insuficiente para o produto {produtoCompra.NomeProduto}.";
                        return RedirectToAction("Index");
                    }

                    // Alterando a quantidade de produto no ProdutoModel
                    produto.Quantidade += produtoCompra.Quantidade;
                    _produtoRepositorio.Alterar(produto);

                    // Alterando o valor total da minha compra
                    var valorTotal = produtoCompra.ValorProduto * produtoCompra.Quantidade;
                    compra.Valor += valorTotal;
                    _compraRepositorio.Alterar(compra);


                    // Adicionando o produto
                    _compraRepositorio.AdicionarProdutoCompra(produtoCompra);

                    TempData["MensagemSucesso"] = $"O {produtoCompra.NomeProduto} foi adicionado a compra!";
                }

                return RedirectToAction("Index");
            } 
            catch(Exception err){
                TempData["MensagemErro"] = $"Ops, não conseguimos adicionar o Produto à sua compra. Detalhes do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult EditarProdutoCompra(int compraId, int id)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            ViewBag.Produtos = new SelectList(_produtoRepositorio.ListarPorUsuarioId(usuarioLogado.Id), "Id", "Nome");
            ViewBag.CompraId = compraId;

            var produtoCompra = _compraRepositorio.ListarProdutoCompraPorId(id);

            if (produtoCompra == null || produtoCompra.CompraId != compraId)
            {
                return NotFound();
            }

            ViewBag.FormasDePagamento = ObterFormasDePagamento();
            return View(produtoCompra);
        }

        [HttpPost]
        public IActionResult EditarProdutoCompra(int compraId, ProdutoCompraModel produtoCompra)
        {
            try
            {
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                var produtoCompraAtual = _compraRepositorio.ListarProdutoCompraPorId(produtoCompra.Id);

                if (produtoCompraAtual == null)
                {
                    TempData["MensagemErro"] = "Produto da compra não encontrado.";
                    return RedirectToAction("Detalhes", new { id = compraId });
                }

                var produto = _produtoRepositorio.ListarPorId(produtoCompra.ProdutoId);

                if (produto == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado no estoque.";
                    return RedirectToAction("Detalhes", new { id = compraId });
                }

                produtoCompra.NomeProduto = produto.Nome;

                var diferencaQuantidade = produtoCompra.Quantidade - produtoCompraAtual.Quantidade;

                produto.Quantidade += diferencaQuantidade;
                _produtoRepositorio.Alterar(produto);

                produtoCompraAtual.NomeProduto = produto.Nome;
                produtoCompraAtual.Quantidade = produtoCompra.Quantidade;
                produtoCompraAtual.ValorProduto = produtoCompra.ValorProduto;

                _compraRepositorio.AlterarProdutoCompra(produtoCompraAtual);

                var compra = _compraRepositorio.ListarPorId(compraId);

                if (compra != null && compra.ProdutoCompra != null)
                {
                    compra.Valor = 0;

                    Console.WriteLine($"Total de produtos na compra: {compra.ProdutoCompra.Count}");

                    foreach (var itemProdutoCompra in compra.ProdutoCompra)
                    {
                        Console.WriteLine($"Processando produto: {itemProdutoCompra.NomeProduto}, Quantidade: {itemProdutoCompra.Quantidade}, Valor: {itemProdutoCompra.ValorProduto}");

                        var valorTotalProduto = itemProdutoCompra.ValorProduto * itemProdutoCompra.Quantidade;
                        compra.Valor += valorTotalProduto;
                    }
                    _compraRepositorio.Alterar(compra);
                }

                TempData["MensagemSucesso"] = "Produto editado e valor total atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                var innerExceptionMessage = err.InnerException?.Message ?? "Sem detalhes adicionais";
                TempData["MensagemErro"] = $"Ops, não conseguimos editar o produto da compra. Detalhes do erro: {err.Message}. Detalhes internos: {innerExceptionMessage}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ApagarProduto(ProdutoCompraModel produtoCompra)
        {
            _compraRepositorio.DeletarProduto(produtoCompra);

            var compra = _compraRepositorio.ListarPorId(produtoCompra.CompraId);

            if (compra != null && compra.ProdutoCompra != null)
            {
                compra.Valor = 0;

                Console.WriteLine($"Total de produtos na compra: {compra.ProdutoCompra.Count}");

                foreach (var itemProdutoCompra in compra.ProdutoCompra)
                {
                    Console.WriteLine($"Processando produto: {itemProdutoCompra.NomeProduto}, Quantidade: {itemProdutoCompra.Quantidade}, Valor: {itemProdutoCompra.ValorProduto}");

                    var valorTotalProduto = itemProdutoCompra.ValorProduto * itemProdutoCompra.Quantidade;
                    compra.Valor += valorTotalProduto;
                }
                _compraRepositorio.Alterar(compra);
            }

            return RedirectToAction("Index");
        }
    }
}
