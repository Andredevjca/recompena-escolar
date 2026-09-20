using System.ComponentModel.DataAnnotations;
using RecompensaEscolar.Modelos;

namespace RecompensaEscolar.ModelosTela;

public class DadosAcesso
{
    [Required(ErrorMessage = "Informe seu e-mail."), EmailAddress(ErrorMessage = "Informe um e-mail válido.")] public string Email { get; set; } = "";
    [Required(ErrorMessage = "Informe sua senha.")] public string Senha { get; set; } = "";
    public bool LembrarMe { get; set; }
}
