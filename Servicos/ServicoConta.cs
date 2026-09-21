using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Interfaces.Servicos;
using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Servicos;

public class ServicoConta(IRepositorioConta repositorio) : IServicoConta
{

    public async Task<UsuarioAutenticado?> AutenticarAsync(DadosAcesso dados)
    {
        var usuario = await repositorio.ObterCredenciaisAsync(dados.Email);
        if (usuario == null || !Senhas.Verificar(usuario.Email, usuario.SenhaHash, dados.Senha))
            return null;

        return new UsuarioAutenticado(usuario.Id, usuario.Nome, usuario.Email, usuario.Perfil);
    }
}

