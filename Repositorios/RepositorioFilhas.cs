using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using Dapper;
using MySqlConnector;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Repositorios;

public class RepositorioFilhas(Banco banco, EstadoDemonstracao estado, IRepositorioPainel painel) : IRepositorioFilhas
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => painel.ObterPainelAsync(semestreId);

    public async Task CadastrarFilhaAsync(CadastroFilha dados)
    {
        if (banco.ModoDemonstracao) { await estado.Semaforo.WaitAsync(); try { estado.Painel.Filhas.Add(new(estado.Painel.Filhas.Count + 1, dados.Nome.Trim(), dados.Escolaridade.Trim())); } finally { estado.Semaforo.Release(); } return; }
        await using var conexao = banco.CriarConexao();
        await conexao.ExecuteAsync("INSERT INTO filhas (Nome,Escolaridade) VALUES (@Nome,@Escolaridade)", new { Nome = dados.Nome.Trim(), Escolaridade = dados.Escolaridade.Trim() });
    }

    public async Task<List<Nota>> ObterNotasAsync(int boletimId)
    {
        if (banco.ModoDemonstracao) { await estado.Semaforo.WaitAsync(); try { return estado.Notas.TryGetValue(boletimId, out var itens) ? [.. itens] : []; } finally { estado.Semaforo.Release(); } }
        await using var conexao = banco.CriarConexao();
        return (await conexao.QueryAsync<Nota>("SELECT d.Nome Disciplina,n.Valor FROM notas n JOIN disciplinas d ON d.Id=n.DisciplinaId WHERE n.BoletimId=@boletimId ORDER BY d.Nome", new { boletimId })).ToList();
    }
}
