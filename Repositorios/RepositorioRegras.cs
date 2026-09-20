using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioRegras(Banco banco, EstadoDemonstracao estado, IRepositorioPainel painel) : IRepositorioRegras
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => painel.ObterPainelAsync(semestreId);

    public async Task SalvarRegraAsync(int id, decimal valorRecompensa)
    {
        if (banco.ModoDemonstracao) { await estado.Semaforo.WaitAsync(); try { var indice = estado.Painel.Regras.FindIndex(x => x.Id == id); if (indice < 0) throw new InvalidOperationException("Faixa não encontrada."); estado.Painel.Regras[indice] = estado.Painel.Regras[indice] with { Valor = valorRecompensa }; } finally { estado.Semaforo.Release(); } return; }
        await using var conexao = banco.CriarConexao();
        await conexao.ExecuteAsync("UPDATE regras_recompensa SET Valor=@valorRecompensa WHERE Id=@id", new { id, valorRecompensa });
    }
}
