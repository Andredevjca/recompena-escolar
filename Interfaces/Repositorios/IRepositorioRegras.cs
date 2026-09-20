using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioRegras
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
    Task SalvarRegraAsync(int id, decimal valorRecompensa);
}
