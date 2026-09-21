using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioRecompensas(Banco banco, IRepositorioPainel painel) : IRepositorioRecompensas
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => painel.ObterPainelAsync(semestreId);

    public async Task RegistrarPagamentoAsync(int boletimId, int usuarioId)
    {

        await using var conexao = banco.CriarConexao();
        await conexao.ExecuteAsync("INSERT INTO pagamentos (RecompensaId,UsuarioId) SELECT Id,@usuarioId FROM recompensas WHERE BoletimId=@boletimId ON DUPLICATE KEY UPDATE Id=Id", new { boletimId, usuarioId });
    }
}
