using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioConta(Banco banco) : IRepositorioConta
{

    public async Task<UsuarioEscolar?> ObterCredenciaisAsync(string email)
    {
        UsuarioEscolar? usuario;
        await using var conexao = banco.CriarConexao(); usuario = await conexao.QuerySingleOrDefaultAsync<UsuarioEscolar>("SELECT * FROM usuarios WHERE Email=@Email", new { Email = email.Trim() });
        return usuario;
    }
}
