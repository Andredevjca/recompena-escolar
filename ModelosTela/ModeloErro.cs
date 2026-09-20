namespace RecompensaEscolar.ModelosTela;

public class ModeloErro
{
    public string? IdentificadorRequisicao { get; set; }

    public bool ExibirIdentificadorRequisicao => !string.IsNullOrEmpty(IdentificadorRequisicao);
}
