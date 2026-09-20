using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Servicos;

public class ServicoUsuarios(IRepositorioUsuarios repositorio) : IServicoUsuarios
{
    public Task<List<UsuarioEscolar>> ListarAsync() => repositorio.ListarUsuariosAsync();

    public Task CadastrarAsync(CadastroUsuario dados)
    {
        if (dados.Perfil != "Administrador" && dados.Perfil != "Responsável")
            throw new InvalidOperationException("Perfil inválido.");
        return repositorio.CadastrarUsuarioAsync(dados);
    }
}
