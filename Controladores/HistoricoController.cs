using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Controladores;

[Authorize]
public class HistoricoController(IServicoHistorico servico) : Controller
{
    public async Task<IActionResult> Historico(int? semestre) => View(await servico.ObterPainelAsync(semestre));
}
