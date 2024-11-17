using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class ImagemController : Controller
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public ImagemController(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public IActionResult ObterImagem(int id)
    {
        var usuario = _usuarioRepositorio.ObterPorId(id);
        if (usuario != null && usuario.Logo != null)
        {
            return File(usuario.Logo, "image/jpeg");
        }

        return NotFound();
    }
}
