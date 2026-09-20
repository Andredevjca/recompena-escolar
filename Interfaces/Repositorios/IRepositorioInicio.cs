using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioInicio
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
}
