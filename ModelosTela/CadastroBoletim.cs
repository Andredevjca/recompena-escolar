using System.ComponentModel.DataAnnotations;
using RecompensaEscolar.Modelos;

namespace RecompensaEscolar.ModelosTela;

public class CadastroBoletim
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecione uma filha cadastrada.")]
    public int FilhaId { get; set; }
    [Range(1,int.MaxValue,ErrorMessage="Selecione um semestre cadastrado para esta filha.")]
    public int SemestreId { get; set; }

    [Range(2020, 2100, ErrorMessage = "Informe um ano entre 2020 e 2100.")]
    public int Ano { get; set; } = DateTime.Today.Year;

    [Range(1, 2, ErrorMessage = "Selecione o primeiro ou o segundo semestre.")]
    public int Numero { get; set; } = DateTime.Today.Month <= 6 ? 1 : 2;
    public string[] Disciplinas { get; set; } = [];
    public string[] Valores { get; set; } = [];
}
