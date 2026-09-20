using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoFilhas
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
    Task CadastrarAsync(CadastroFilha dados);
    Task<List<Nota>> ObterNotasAsync(int boletimId);
}
