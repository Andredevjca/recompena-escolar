using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Interfaces.Servicos;

namespace RecompensaEscolar.Servicos;

public class ServicoRegras(IRepositorioRegras repositorio) : IServicoRegras
{
    public Task<ModeloPainel> ObterPainelAsync(int? semestreId = null) => repositorio.ObterPainelAsync(semestreId);

    public async Task<bool> SalvarAsync(int id, string? valorRecompensa)
    {
        if (!decimal.TryParse(valorRecompensa?.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var valor) || valor < 0 || valor > 99999999)
            throw new InvalidOperationException("Informe um valor válido, maior ou igual a zero.");
        if (!(await repositorio.ObterPainelAsync()).Regras.Any(x => x.Id == id)) return false;
        await repositorio.SalvarRegraAsync(id, valor);
        return true;
    }
}
