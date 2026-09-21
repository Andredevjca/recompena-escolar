using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoConta
{
    Task<UsuarioAutenticado?> AutenticarAsync(DadosAcesso dados);
}

