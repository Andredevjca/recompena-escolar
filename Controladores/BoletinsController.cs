using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Controladores;

[Authorize]
public class BoletinsController(IServicoBoletins servico) : Controller
{
    public async Task<IActionResult> Boletins()
    {
        ViewBag.Filhas = await servico.ListarFilhasAsync();
        return View(new CadastroBoletim());
    }
    [HttpPost]
    public async Task<IActionResult> Boletins(CadastroBoletim dados)
    {
        ViewBag.Filhas = await servico.ListarFilhasAsync();
        if (ModelState.IsValid)
        {
            try
            {
                await servico.CadastrarAsync(dados);
                TempData["Sucesso"] = "Boletim lançado! A recompensa foi calculada.";
                return RedirectToAction("Historico", "Historico");
            }
            catch (InvalidOperationException excecao)
            {
                ModelState.AddModelError("", excecao.Message);
            }
        }
        return View(dados);
    }
}
