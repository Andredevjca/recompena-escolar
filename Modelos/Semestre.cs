namespace RecompensaEscolar.Modelos;

public record Semestre(int Id, int Ano, int Numero)
{
    public string Descricao => $"{Ano} • {Numero}º Semestre";
}
