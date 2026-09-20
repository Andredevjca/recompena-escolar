using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoRecompensas
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
    Task<bool> PagarAsync(int id, int usuarioId);
}
