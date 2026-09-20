using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Interfaces.Servicos;

public interface IServicoBoletins
{
    Task<List<Filha>> ListarFilhasAsync();
    Task CadastrarAsync(CadastroBoletim dados);
}
