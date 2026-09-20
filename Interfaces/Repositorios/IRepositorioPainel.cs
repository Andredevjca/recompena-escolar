using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioPainel
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
}
