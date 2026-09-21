namespace RecompensaEscolar.Modelos;
public record PeriodoEscolar(int FilhaId, int SemestreId, string Serie, int Ano, int Numero) { public string Descricao => $"{Serie} · {Ano} · {Numero}º semestre"; }
