using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoUsuarios
{
    Task<List<UsuarioEscolar>> ListarAsync();
    Task CadastrarAsync(CadastroUsuario dados);
}
