using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Controladores;

[Authorize]
public class RecompensasController(IServicoRecompensas servico) : Controller
{
    public async Task<IActionResult> Recompensas(int? semestre) => View(await servico.ObterPainelAsync(semestre));
    [HttpPost]
    public async Task<IActionResult> Pagar(int id, int? semestre)
    {
        if (!await servico.PagarAsync(id, int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))) return NotFound();
        TempData["Sucesso"] = "Pagamento registrado. Hora de celebrar essa conquista!";
        return RedirectToAction(nameof(Recompensas), new { semestre });
    }
}
