using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioHistorico
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
}
