using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoHistorico
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
}
