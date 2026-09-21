using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioBoletins(Banco banco, IRepositorioPainel painel) : IRepositorioBoletins
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => painel.ObterPainelAsync(semestreId);

    public async Task CadastrarBoletimAsync(CadastroBoletim dados, List<Nota> itens)
    {
        var media = Math.Round(itens.Average(x => x.Valor), 2, MidpointRounding.AwayFromZero);

        await using var conexao = banco.CriarConexao();
        await conexao.OpenAsync();
        await using var transacao = await conexao.BeginTransactionAsync();
        await conexao.ExecuteAsync("INSERT IGNORE INTO semestres (Ano,Numero) VALUES (@Ano,@Numero)", dados, transacao);
        var semestreId = await conexao.QuerySingleAsync<int>("SELECT Id FROM semestres WHERE Ano=@Ano AND Numero=@Numero", dados, transacao);
        if (await conexao.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM boletins WHERE FilhaId=@FilhaId AND SemestreId=@semestreId", new { dados.FilhaId, semestreId }, transacao) > 0)
            throw new InvalidOperationException("Já existe um boletim para esta filha neste semestre.");
        var boletimId = await conexao.ExecuteScalarAsync<int>("INSERT INTO boletins (FilhaId,SemestreId) VALUES (@FilhaId,@semestreId); SELECT LAST_INSERT_ID();", new { dados.FilhaId, semestreId }, transacao);
        foreach (var nota in itens)
        {
            await conexao.ExecuteAsync("INSERT IGNORE INTO disciplinas (Nome) VALUES (@Disciplina)", nota, transacao);
            await conexao.ExecuteAsync("INSERT INTO notas (BoletimId,DisciplinaId,Valor) SELECT @boletimId,Id,@Valor FROM disciplinas WHERE Nome=@Disciplina", new { boletimId, nota.Disciplina, nota.Valor }, transacao);
        }
        var regras = await conexao.QueryAsync<RegraRecompensa>("SELECT * FROM regras_recompensa", transaction: transacao);
        await conexao.ExecuteAsync("INSERT INTO recompensas (BoletimId,Valor) VALUES (@boletimId,@Valor)", new { boletimId, Valor = ServicoRecompensas.Calcular(media, regras) }, transacao);
        await transacao.CommitAsync();
    }
}
