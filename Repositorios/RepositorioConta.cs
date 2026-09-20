using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioConta(Banco banco, EstadoDemonstracao estado) : IRepositorioConta
{
    public bool ModoDemonstracao => banco.ModoDemonstracao;

    public async Task<UsuarioEscolar?> ObterCredenciaisAsync(string email)
    {
        UsuarioEscolar? usuario;
        if (banco.ModoDemonstracao) { await estado.Semaforo.WaitAsync(); try { usuario = estado.Usuarios.FirstOrDefault(x => x.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)); } finally { estado.Semaforo.Release(); } }
        else { await using var conexao = banco.CriarConexao(); usuario = await conexao.QuerySingleOrDefaultAsync<UsuarioEscolar>("SELECT * FROM usuarios WHERE Email=@Email", new { Email = email.Trim() }); }
        return usuario;
    }
}
