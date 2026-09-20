using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Controladores;

[Authorize]
public class FilhasController(IServicoFilhas servico) : Controller
{
    public async Task<IActionResult> Filhas(int? semestre) => View(await servico.ObterPainelAsync(semestre));
    public async Task<IActionResult> Detalhes(int id, int? semestre)
    {
        var modelo = await servico.ObterPainelAsync(semestre);
        var filha = modelo.Filhas.FirstOrDefault(x => x.Id == id);
        if (filha == null) return NotFound();
        ViewBag.Filha = filha;
        var boletim = modelo.Atual(id);
        ViewBag.Notas = boletim == null ? new List<Nota>() : await servico.ObterNotasAsync(boletim.Id);
        return View(modelo);
    }
    public IActionResult NovaFilha() => View(new CadastroFilha());
    [HttpPost]
    public async Task<IActionResult> NovaFilha(CadastroFilha dados)
    {
        if (!ModelState.IsValid) return View(dados);
        await servico.CadastrarAsync(dados);
        TempData["Sucesso"] = "Filha cadastrada. Uma nova jornada começa!";
        return RedirectToAction(nameof(Filhas));
    }
}
