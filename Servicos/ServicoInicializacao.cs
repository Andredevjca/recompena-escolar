using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
namespace RecompensaEscolar.Servicos;

public class ServicoInicializacao(Banco banco, EstadoDemonstracao demonstracao, IRepositorioInicializacao inicializacao)
{
    public async Task InicializarAsync()
    {
        if (banco.ModoDemonstracao) { demonstracao.Inicializar(); return; }
        await inicializacao.InicializarAsync();
    }
}
