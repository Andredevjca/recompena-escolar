using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoInicio
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
}
