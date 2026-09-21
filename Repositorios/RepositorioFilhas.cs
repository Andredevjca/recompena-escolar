using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioFilhas(Banco banco, IRepositorioPainel painel) : IRepositorioFilhas
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => painel.ObterPainelAsync(semestreId);

    public async Task CadastrarFilhaAsync(CadastroFilha dados)
    {

        await using var conexao = banco.CriarConexao();
        await conexao.ExecuteAsync("INSERT INTO filhas (Nome,Escolaridade) VALUES (@Nome,@Escolaridade)", new { Nome = dados.Nome.Trim(), Escolaridade = dados.Escolaridade.Trim() });
    }

    public async Task<List<Nota>> ObterNotasAsync(int boletimId)
    {

        await using var conexao = banco.CriarConexao();
        return (await conexao.QueryAsync<Nota>("SELECT d.Nome Disciplina,n.Valor FROM notas n JOIN disciplinas d ON d.Id=n.DisciplinaId WHERE n.BoletimId=@boletimId ORDER BY d.Nome", new { boletimId })).ToList();
    }
}
