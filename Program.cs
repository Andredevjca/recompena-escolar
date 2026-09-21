using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;
using System.Threading.RateLimiting;
using RecompensaEscolar.Servicos;
using RecompensaEscolar.Dependencias;

var construtor = WebApplication.CreateBuilder(args);
construtor.Configuration.AddJsonFile("appsettings.Local.json", optional: true).AddEnvironmentVariables();
var protecao = construtor.Services.AddDataProtection().SetApplicationName("RecompensaEscolar")
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(construtor.Environment.ContentRootPath, "Dados", "Chaves")));
if (OperatingSystem.IsWindows()) protecao.ProtectKeysWithDpapi();
construtor.Services.AddControllersWithViews(opcoes =>
{
    opcoes.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    opcoes.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(campo => $"O campo {campo} deve ser numérico.");
    opcoes.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Informe um valor para este campo.");
    opcoes.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((valor, campo) => $"O valor informado para {campo} é inválido.");
});
construtor.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opcoes =>
{
    opcoes.LoginPath = "/Conta/Entrar";
    opcoes.ReturnUrlParameter = "urlRetorno";
    opcoes.AccessDeniedPath = "/Conta/AcessoNegado";
    opcoes.Cookie.Name = "RecompensaEscolar.Sessao";
    opcoes.ExpireTimeSpan = TimeSpan.FromHours(8);
});
construtor.Services.AddAuthorization();
construtor.Services.AddRateLimiter(opcoes =>
{
    opcoes.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    opcoes.AddPolicy("acesso", contexto => RateLimitPartition.GetFixedWindowLimiter(
        contexto.Connection.RemoteIpAddress?.ToString() ?? "desconhecido",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
});
construtor.Services.AdicionarDependencias();
var aplicacao = construtor.Build();
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("pt-BR");
if (!aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseExceptionHandler("/Inicio/Erro");
    aplicacao.UseHsts();
    aplicacao.UseHttpsRedirection();
}
await using (var escopo = aplicacao.Services.CreateAsyncScope())
    await escopo.ServiceProvider.GetRequiredService<ServicoInicializacao>().InicializarAsync();
aplicacao.UseStaticFiles();
aplicacao.UseRouting();
aplicacao.UseRateLimiter();
aplicacao.UseAuthentication();
aplicacao.UseAuthorization();
aplicacao.MapControllerRoute("padrao", "{controller=Inicio}/{action=Inicio}/{id?}");
aplicacao.Run();
