using System.ComponentModel.DataAnnotations;
using RecompensaEscolar.Modelos;

namespace RecompensaEscolar.ModelosTela;

public class CadastroUsuario
{
    [Required(ErrorMessage = "Informe o nome do usuário.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(180, ErrorMessage = "O e-mail deve ter até 180 caracteres.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Informe a senha inicial.")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 128 caracteres.")]
    public string Senha { get; set; } = "";
    public string Perfil { get; set; } = "Responsável";
}
