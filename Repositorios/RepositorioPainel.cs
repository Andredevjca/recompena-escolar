using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioPainel(Banco banco, EstadoDemonstracao estado) : IRepositorioPainel
{
    public async Task<ModeloPainel> ObterPainelAsync(int? semestreId = null)
    {
        ModeloPainel modelo;
        if (banco.ModoDemonstracao)
        {
            await estado.Semaforo.WaitAsync();
            try { modelo = new() { Filhas = [.. estado.Painel.Filhas], Semestres = [.. estado.Painel.Semestres], Boletins = [.. estado.Painel.Boletins], Regras = [.. estado.Painel.Regras], Demonstracao = true }; }
            finally { estado.Semaforo.Release(); }
        }
        else
        {
            await using var conexao = banco.CriarConexao();
            modelo = new()
            {
                Filhas = (await conexao.QueryAsync<Filha>("SELECT * FROM filhas ORDER BY Id")).ToList(),
                Semestres = (await conexao.QueryAsync<Semestre>("SELECT * FROM semestres ORDER BY Ano,Numero")).ToList(),
                Regras = (await conexao.QueryAsync<RegraRecompensa>("SELECT * FROM regras_recompensa ORDER BY MediaMinima")).ToList(),
                Boletins = (await conexao.QueryAsync<Boletim>("""
                    SELECT b.Id,b.FilhaId,b.SemestreId,CAST(ROUND(AVG(n.Valor),2) AS DECIMAL(4,2)) MediaGeral,
                    r.Valor,EXISTS(SELECT 1 FROM pagamentos p WHERE p.RecompensaId=r.Id) Pago
                    FROM boletins b JOIN notas n ON n.BoletimId=b.Id JOIN recompensas r ON r.BoletimId=b.Id
                    GROUP BY b.Id,b.FilhaId,b.SemestreId,r.Id,r.Valor
                    """)).ToList()
            };
        }
        modelo.SemestreSelecionadoId = semestreId.HasValue && modelo.Semestres.Any(x => x.Id == semestreId) ? semestreId.Value : modelo.Semestres.LastOrDefault()?.Id ?? 0;
        return modelo;
    }
}
