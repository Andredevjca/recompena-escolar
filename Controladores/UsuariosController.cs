using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Controladores;

[Authorize]
public class UsuariosController(IServicoUsuarios servico) : Controller
{
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Usuarios()
    {
        ViewBag.Usuarios = await servico.ListarAsync();
        return View(new CadastroUsuario());
    }
    [HttpPost, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Usuarios(CadastroUsuario dados)
    {
        if (ModelState.IsValid)
        {
            try { await servico.CadastrarAsync(dados); TempData["Sucesso"] = "Usuário cadastrado."; return RedirectToAction(nameof(Usuarios)); }
            catch (InvalidOperationException excecao) { ModelState.AddModelError("", excecao.Message); }
        }
        ViewBag.Usuarios = await servico.ListarAsync();
        return View(dados);
    }
}
