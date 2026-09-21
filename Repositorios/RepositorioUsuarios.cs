using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioUsuarios(Banco banco) : IRepositorioUsuarios
{
    public async Task<List<UsuarioEscolar>> ListarUsuariosAsync()
    {

        await using var conexao = banco.CriarConexao();
        return (await conexao.QueryAsync<UsuarioEscolar>("SELECT Id,Nome,Email,'' SenhaHash,Perfil FROM usuarios ORDER BY Nome")).ToList();
    }

    public async Task CadastrarUsuarioAsync(CadastroUsuario dados)
    {
        var email = dados.Email.Trim().ToLowerInvariant();
        var resumoSenha = Senhas.Gerar(email, dados.Senha);

        await using var conexao = banco.CriarConexao();
        try { await conexao.ExecuteAsync("INSERT INTO usuarios (Nome,Email,SenhaHash,Perfil) VALUES (@Nome,@email,@resumoSenha,@Perfil)", new { dados.Nome, email, resumoSenha, dados.Perfil }); }
        catch (MySqlException excecao) when (excecao.Number == 1062) { throw new InvalidOperationException("Este e-mail já está cadastrado."); }
    }
}
