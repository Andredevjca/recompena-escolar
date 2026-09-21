using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Repositorios;
using RecompensaEscolar.ModelosTela;
namespace RecompensaEscolar.Controladores;
[Authorize]
public class PeriodosController(RepositorioPeriodos periodos,IRepositorioPainel painel):Controller {
 public async Task<IActionResult> Periodos(int? filhaId)=>await Exibir(new CadastroPeriodo{FilhaId=filhaId??0});
 [HttpPost] public async Task<IActionResult> Periodos(CadastroPeriodo dados) {
  if(ModelState.IsValid) { try { await periodos.CadastrarAsync(dados); TempData["Sucesso"]="Série e semestres cadastrados. Você já pode lançar as notas."; return RedirectToAction(nameof(Periodos),new{dados.FilhaId}); } catch(InvalidOperationException erro){ModelState.AddModelError("",erro.Message);} }
  return await Exibir(dados);
 }
 private async Task<IActionResult> Exibir(CadastroPeriodo dados) { var modelo=await painel.ObterPainelAsync(); dados.Filhas=modelo.Filhas; dados.Boletins=modelo.Boletins; dados.Periodos=await periodos.ListarAsync(); return View("Periodos",dados); }
}