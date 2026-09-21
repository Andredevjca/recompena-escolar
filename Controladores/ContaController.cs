using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Servicos;
using Microsoft.AspNetCore.RateLimiting;
namespace RecompensaEscolar.Controladores;

public class ContaController(IServicoConta autenticacao) : Controller
{
    [HttpGet]
    public IActionResult Entrar(string? urlRetorno)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Inicio", "Inicio");
        ViewBag.UrlRetorno = urlRetorno;
        return View(new DadosAcesso());
    }
    [HttpPost, EnableRateLimiting("acesso")]
    public async Task<IActionResult> Entrar(DadosAcesso dados, string? urlRetorno)
    {
        ViewBag.UrlRetorno = urlRetorno;
        if (!ModelState.IsValid) return View(dados);
        var usuario = await autenticacao.AutenticarAsync(dados);
        if (usuario == null) { ModelState.AddModelError("", "E-mail ou senha incorretos."); return View(dados); }
        var identidade = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()), new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email), new Claim(ClaimTypes.Role, usuario.Perfil)
        }, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidade),
            new AuthenticationProperties { IsPersistent = dados.LembrarMe });
        return Url.IsLocalUrl(urlRetorno) ? LocalRedirect(urlRetorno!) : RedirectToAction("Inicio", "Inicio");
    }
    [HttpPost, Authorize]
    public async Task<IActionResult> Sair()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Entrar));
    }
    public IActionResult AcessoNegado() { Response.StatusCode = 403; return View(); }
}

