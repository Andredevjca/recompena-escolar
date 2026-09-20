using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Servicos;

public class ServicoFilhas(IRepositorioFilhas repositorio) : IServicoFilhas
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => repositorio.ObterPainelAsync(semestreId);

    public Task CadastrarAsync(CadastroFilha dados) => repositorio.CadastrarFilhaAsync(dados);

    public Task<List<Nota>> ObterNotasAsync(int boletimId) => repositorio.ObterNotasAsync(boletimId);
}
