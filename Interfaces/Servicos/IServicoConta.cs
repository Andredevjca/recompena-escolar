using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoConta
{
    bool ModoDemonstracao { get; }
    Task<UsuarioAutenticado?> AutenticarAsync(DadosAcesso dados);
}

