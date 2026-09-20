using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioConta
{
    bool ModoDemonstracao { get; }
    Task<UsuarioEscolar?> ObterCredenciaisAsync(string email);
}
