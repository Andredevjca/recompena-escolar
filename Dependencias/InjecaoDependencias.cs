using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Interfaces.Servicos;
using RecompensaEscolar.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Dependencias;

public static class InjecaoDependencias
{
    public static IServiceCollection AdicionarDependencias(this IServiceCollection servicos)
    {
        servicos.AddSingleton<Banco>();
        servicos.AddScoped<RepositorioPeriodos>();
        servicos.AddSingleton<EstadoDemonstracao>();
        servicos.AddScoped<IRepositorioPainel, RepositorioPainel>();
        servicos.AddScoped<IRepositorioConta, RepositorioConta>();
        servicos.AddScoped<IRepositorioFilhas, RepositorioFilhas>();
        servicos.AddScoped<IRepositorioBoletins, RepositorioBoletins>();
        servicos.AddScoped<IRepositorioInicio, RepositorioInicio>();
        servicos.AddScoped<IRepositorioHistorico, RepositorioHistorico>();
        servicos.AddScoped<IRepositorioRecompensas, RepositorioRecompensas>();
        servicos.AddScoped<IRepositorioRegras, RepositorioRegras>();
        servicos.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();
        servicos.AddScoped<IServicoConta, ServicoConta>();
        servicos.AddScoped<IServicoFilhas, ServicoFilhas>();
        servicos.AddScoped<IServicoBoletins, ServicoBoletins>();
        servicos.AddScoped<IServicoInicio, ServicoInicio>();
        servicos.AddScoped<IServicoHistorico, ServicoHistorico>();
        servicos.AddScoped<IServicoRecompensas, ServicoRecompensas>();
        servicos.AddScoped<IServicoRegras, ServicoRegras>();
        servicos.AddScoped<IServicoUsuarios, ServicoUsuarios>();
        servicos.AddScoped<IRepositorioInicializacao, RepositorioInicializacao>();
        servicos.AddScoped<ServicoInicializacao>();
        return servicos;
    }
}
