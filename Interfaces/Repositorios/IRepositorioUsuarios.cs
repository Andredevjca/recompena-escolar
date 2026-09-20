using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioUsuarios
{
    Task<List<UsuarioEscolar>> ListarUsuariosAsync();
    Task CadastrarUsuarioAsync(CadastroUsuario dados);
}
