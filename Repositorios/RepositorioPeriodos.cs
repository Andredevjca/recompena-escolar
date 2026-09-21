using Dapper;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
namespace RecompensaEscolar.Repositorios;

public class RepositorioPeriodos(Banco banco)
{
    public async Task<List<PeriodoEscolar>> ListarAsync()
    {

        await using var c = banco.CriarConexao();
        return (await c.QueryAsync<PeriodoEscolar>("SELECT p.FilhaId,p.SemestreId,p.Serie,s.Ano,s.Numero FROM periodos_escolares p JOIN semestres s ON s.Id=p.SemestreId ORDER BY s.Ano DESC,s.Numero")).ToList();
    }
    public async Task CadastrarAsync(CadastroPeriodo dados)
    {
        var numeros = new[] { dados.Primeiro ? 1 : 0, dados.Segundo ? 2 : 0, dados.Terceiro ? 3 : 0, dados.Quarto ? 4 : 0 }.Where(x => x > 0).ToArray();
        if (numeros.Length == 0 || string.IsNullOrWhiteSpace(dados.Serie)) throw new InvalidOperationException("Informe a série e selecione pelo menos um semestre.");

        await using var c = banco.CriarConexao(); await c.OpenAsync(); await using var t = await c.BeginTransactionAsync();
        if (await c.QuerySingleOrDefaultAsync<int?>("SELECT Id FROM filhas WHERE Id=@FilhaId FOR UPDATE", dados, t) == null) throw new InvalidOperationException("Filha não encontrada.");
        var series = await c.QueryAsync<string>("SELECT p.Serie FROM periodos_escolares p JOIN semestres s ON s.Id=p.SemestreId WHERE p.FilhaId=@FilhaId AND s.Ano=@Ano", dados, t);
        if (series.Any(x => x != dados.Serie.Trim())) throw new InvalidOperationException("Já existe outra série cadastrada para esta filha neste ano.");
        foreach (var numero in numeros)
        {
            await c.ExecuteAsync("INSERT IGNORE INTO semestres (Ano,Numero) VALUES (@Ano,@numero)", new { dados.Ano, numero }, t);
            await c.ExecuteAsync("INSERT IGNORE INTO periodos_escolares (FilhaId,SemestreId,Serie) SELECT @FilhaId,Id,@Serie FROM semestres WHERE Ano=@Ano AND Numero=@numero", new { dados.FilhaId, dados.Ano, numero, Serie = dados.Serie.Trim() }, t);
        }
        await t.CommitAsync();
    }
}