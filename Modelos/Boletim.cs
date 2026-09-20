namespace RecompensaEscolar.Modelos;

public record Boletim(int Id, int FilhaId, int SemestreId, decimal MediaGeral, decimal Valor, bool Pago)
{
    // O Dapper converte o resultado numérico do EXISTS do MySQL para esta propriedade booleana.
    public Boletim() : this(0, 0, 0, 0, 0, false) { }
}
