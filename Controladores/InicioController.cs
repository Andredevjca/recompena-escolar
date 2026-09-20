using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Controladores;

[Authorize]
public class InicioController(IServicoInicio servico) : Controller
{
    public async Task<IActionResult> Inicio(int? semestre) => View(await servico.ObterPainelAsync(semestre));
    [AllowAnonymous, ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Erro() => View(new ModeloErro { IdentificadorRequisicao = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
