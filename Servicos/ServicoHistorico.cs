using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Servicos;

public class ServicoHistorico(IRepositorioHistorico repositorio) : IServicoHistorico
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => repositorio.ObterPainelAsync(semestreId);
}
