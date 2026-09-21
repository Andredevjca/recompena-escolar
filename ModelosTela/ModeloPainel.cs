using System.ComponentModel.DataAnnotations;
using RecompensaEscolar.Modelos;

namespace RecompensaEscolar.ModelosTela;

public class ModeloPainel
{
    public List<Filha> Filhas { get; set; } = [];
    public List<Semestre> Semestres { get; set; } = [];
    public List<Boletim> Boletins { get; set; } = [];
    public List<RegraRecompensa> Regras { get; set; } = [];
    public int SemestreSelecionadoId { get; set; }
    public Semestre? SemestreSelecionado => Semestres.FirstOrDefault(x => x.Id == SemestreSelecionadoId);
    public IEnumerable<Boletim> BoletinsAtuais => Boletins.Where(x => x.SemestreId == SemestreSelecionadoId);
    public decimal Total => BoletinsAtuais.Sum(x => x.Valor);
    public Boletim? Atual(int filhaId) => BoletinsAtuais.FirstOrDefault(x => x.FilhaId == filhaId);
    public decimal? Variacao(int filhaId)
    {
        var atual = Atual(filhaId);
        var selecionado = SemestreSelecionado;
        var identificadoresAnteriores = Semestres.Where(x => selecionado != null && (x.Ano < selecionado.Ano || (x.Ano == selecionado.Ano && x.Numero < selecionado.Numero)))
            .OrderByDescending(x => x.Ano).ThenByDescending(x => x.Numero).Select(x => x.Id);
        var anterior = identificadoresAnteriores.Select(id => Boletins.FirstOrDefault(r => r.FilhaId == filhaId && r.SemestreId == id)).FirstOrDefault(r => r != null);
        return atual == null || anterior == null ? null : atual.MediaGeral - anterior.MediaGeral;
    }
}
