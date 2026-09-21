using System.Text.RegularExpressions;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioInicializacao(IConfiguration configuracao, IWebHostEnvironment ambiente, Banco banco) : IRepositorioInicializacao
{
    public async Task InicializarAsync()
    {
        var cadeiaConexao = configuracao.GetConnectionString("MySql");
        if (string.IsNullOrWhiteSpace(cadeiaConexao)) throw new InvalidOperationException("Configure ConnectionStrings:MySql ou ative ModoDemonstracao no ambiente Development.");
        var configuracoes = new MySqlConnectionStringBuilder(cadeiaConexao);
        var nomeBanco = configuracoes.Database;
        if (!Regex.IsMatch(nomeBanco, "^[a-zA-Z0-9_]+$")) throw new InvalidOperationException("Nome de banco inválido.");
        configuracoes.Database = "";
        await using (var servidor = new MySqlConnection(configuracoes.ConnectionString))
        {
            await servidor.OpenAsync();
            await servidor.ExecuteAsync($"CREATE DATABASE IF NOT EXISTS `{nomeBanco}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");
        }
        await using var conexao = banco.CriarConexao();
        await conexao.OpenAsync();
        await AtualizadorEstrutura.TraduzirColunasAsync(conexao);
        var estrutura = await File.ReadAllTextAsync(Path.Combine(ambiente.ContentRootPath, "Dados", "estrutura.sql"));
        await conexao.ExecuteAsync(estrutura);
        await AtualizadorEstrutura.AmpliarSemestresAsync(conexao);
        await conexao.ExecuteAsync("INSERT INTO usuarios (Nome,Email,SenhaHash,Perfil) VALUES (@Nome,@Email,@SenhaHash,'Administrador') ON DUPLICATE KEY UPDATE Id=Id",
            new { Nome = "André da Silva Ramos", Email = "admin@admin.com", SenhaHash = Senhas.Gerar("admin@admin.com", configuracao["SenhaAdministradorInicial"] ?? "admin") });
        await conexao.ExecuteAsync("INSERT IGNORE INTO regras_recompensa (MediaMinima,Valor) VALUES (0,0),(7,100),(8,180),(9,250),(10,300)");
    }
}
