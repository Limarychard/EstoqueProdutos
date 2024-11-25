using Microsoft.AspNetCore.Mvc;

namespace EstoqueProdutos.Controllers
{
    public class ConfiguracaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
