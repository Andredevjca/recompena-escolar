using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioRegras(Banco banco, IRepositorioPainel painel) : IRepositorioRegras
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => painel.ObterPainelAsync(semestreId);

    public async Task SalvarRegraAsync(int id, decimal valorRecompensa)
    {

        await using var conexao = banco.CriarConexao();
        await conexao.ExecuteAsync("UPDATE regras_recompensa SET Valor=@valorRecompensa WHERE Id=@id", new { id, valorRecompensa });
    }
}
