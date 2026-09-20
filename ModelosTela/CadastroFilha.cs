using System.ComponentModel.DataAnnotations;
using RecompensaEscolar.Modelos;

namespace RecompensaEscolar.ModelosTela;

public class CadastroFilha
{
    [Required(ErrorMessage = "Informe o nome da filha.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 80 caracteres.")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "Informe a escolaridade.")]
    [StringLength(100, ErrorMessage = "A escolaridade deve ter até 100 caracteres.")]
    public string Escolaridade { get; set; } = "Ensino Fundamental";
}
