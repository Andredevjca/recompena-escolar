using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoRegras
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
    Task<bool> SalvarAsync(int id, string? valorRecompensa);
}
