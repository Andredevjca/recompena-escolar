using RecompensaEscolar.Interfaces.Repositorios;
namespace RecompensaEscolar.Servicos;
public class ServicoInicializacao(IRepositorioInicializacao inicializacao)
{
    public Task InicializarAsync() => inicializacao.InicializarAsync();
}
