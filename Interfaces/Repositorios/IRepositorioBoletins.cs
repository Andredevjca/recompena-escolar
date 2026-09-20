using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Repositorios;

public interface IRepositorioBoletins
{
    Task<ModeloPainel> ObterPainelAsync(int? semestreId = null);
    Task CadastrarBoletimAsync(CadastroBoletim dados, List<Nota> itens);
}
