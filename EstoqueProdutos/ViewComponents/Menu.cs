using EstoqueProdutos.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EstoqueProdutos.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString("SessaoUsuarioLogado");

            if(sessaoUsuario == null)
            {
                return Content("");
            }

            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

            return View(usuario);
        }
    }
}
