using Microsoft.AspNetCore.Identity;

namespace RecompensaEscolar.Servicos;

public static class Senhas
{
    public static string Gerar(string email, string senha) =>
        new PasswordHasher<string>().HashPassword(email, senha);

    public static bool Verificar(string email, string senhaHash, string senha) =>
        new PasswordHasher<string>().VerifyHashedPassword(email, senhaHash, senha)
            != PasswordVerificationResult.Failed;
}

