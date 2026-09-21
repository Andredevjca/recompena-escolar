using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioConta
{
    Task<UsuarioEscolar?> ObterCredenciaisAsync(string email);
}
