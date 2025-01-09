using EstoqueProdutos.Data;
using EstoqueProdutos.Enums;
using EstoqueProdutos.Filters;
using EstoqueProdutos.Helper;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EstoqueProdutos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ClienteController : Controller
    {
        readonly private IClienteRepositorio _clienteRepositorio;
        readonly private ISessao _sessao;

        public ClienteController(
            IClienteRepositorio clienteRepositorio,
            ISessao sessao
            )
        {
            _clienteRepositorio = clienteRepositorio;
            _sessao = sessao;
        }

        public IActionResult Index(string? Nome = null, long? Telefone = null, long? CPF = null, bool? BomPagador = null, DateTime? DataUltimaCompraInicio = null, DateTime? DataUltimaCompraInicioFim = null)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            List<ClienteModel> Clientes;
            if (usuarioLogado.Perfil == PerfilEnum.Admin)
            {
                Clientes = _clienteRepositorio.ListarTodos();
            }
            else
            {
                Clientes = _clienteRepositorio.ListarPorUsuarioId(usuarioLogado.Id);
            }

            if (!string.IsNullOrEmpty(Nome))
                Clientes = Clientes.Where(u => u.Nome.Contains(Nome, StringComparison.OrdinalIgnoreCase)).ToList();

            if (Telefone != 0)
            {
                string telefoneFiltro = Telefone.ToString();
                Clientes = Clientes.Where(u => u.Telefone.ToString().Contains(telefoneFiltro)).ToList();
            }

            if (CPF != null)
            {
                string CpfFiltro = CPF.ToString();
                Clientes = Clientes.Where(u => u.Telefone.ToString().Contains(CpfFiltro)).ToList();
            }

            if (BomPagador.HasValue)
                Clientes = Clientes.Where(u => u.BomPagador == BomPagador).ToList();

            if (DataUltimaCompraInicio.HasValue)
                Clientes = Clientes.Where(u => u.DtUltCompra >= DataUltimaCompraInicio.Value).ToList();

            if (DataUltimaCompraInicioFim.HasValue)
                Clientes = Clientes.Where(u => u.DtUltCompra <= DataUltimaCompraInicioFim.Value).ToList();

            ViewData["Nome"] = Nome;
            ViewData["Telefone"] = Telefone;
            ViewData["CPF"] = CPF;
            ViewData["BomPagador"] = BomPagador;
            ViewData["DataUltimaCompraInicio"] = DataUltimaCompraInicio?.ToString("yyyy-MM-dd");
            ViewData["DataUltimaCompraInicioFim"] = DataUltimaCompraInicioFim?.ToString("yyyy-MM-dd");

            return View(Clientes);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(ClienteModel cliente)
        {
            try
            {
                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {

                    var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    cliente.UsuarioId = usuarioLogado.Id;

                    _clienteRepositorio.Adicionar(cliente);
                    TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos criar seu novo cliente, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            ClienteModel cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Editar(ClienteModel cliente)
        {
            try
            {
                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {
                    var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    cliente.UsuarioId = usuarioLogado.Id;

                    _clienteRepositorio.Alterar(cliente);
                    TempData["MensagemSucesso"] = "Cliente editado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos editar seu cliente, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(ClienteModel cliente)
        {
            _clienteRepositorio.Deletar(cliente);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Enderecos(int id)
        {
            ClienteModel cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }
    }
}