using EstoqueProdutos.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueProdutos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
