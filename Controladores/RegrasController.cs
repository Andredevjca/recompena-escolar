using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Controladores;

[Authorize]
public class RegrasController(IServicoRegras servico) : Controller
{
    public async Task<IActionResult> Regras() => View(await servico.ObterPainelAsync());

    [HttpPost, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> SalvarRegra(int id, string valorRecompensa)
    {
        try
        {
            if (!await servico.SalvarAsync(id, valorRecompensa)) return NotFound();
            TempData["Sucesso"] = "Regra atualizada. O novo valor vale para os próximos boletins.";
        }
        catch (InvalidOperationException excecao) { TempData["Erro"] = excecao.Message; }
        return RedirectToAction(nameof(Regras));
    }
}
