using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioRecompensas
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
    Task RegistrarPagamentoAsync(int boletimId, int usuarioId);
}
