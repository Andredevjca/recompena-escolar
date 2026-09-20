using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Servicos;

public class ServicoRecompensas(IRepositorioRecompensas repositorio) : IServicoRecompensas
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => repositorio.ObterPainelAsync(semestreId);

    public async Task<bool> PagarAsync(int id, int usuarioId)
    {
        if (!(await repositorio.ObterPainelAsync()).Boletins.Any(x => x.Id == id)) return false;
        await repositorio.RegistrarPagamentoAsync(id, usuarioId);
        return true;
    }

    public static decimal Calcular(decimal media, IEnumerable<RegraRecompensa> regras) =>
        regras.OrderByDescending(regra => regra.MediaMinima)
            .FirstOrDefault(regra => media >= regra.MediaMinima)?.Valor ?? 0;
}
