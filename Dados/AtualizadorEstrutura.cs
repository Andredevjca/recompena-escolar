using Dapper;
using MySqlConnector;

namespace RecompensaEscolar.Dados;

public static class AtualizadorEstrutura
{
    // Os nomes em inglês abaixo existem apenas para reconhecer a estrutura inicial.
    // Renomear as colunas preserva os registros, índices e relacionamentos existentes.
    private static readonly (string Tabela, string NomeAnterior, string NomeAtual)[] Alteracoes =
    [
        ("usuarios", "Name", "Nome"),
        ("usuarios", "PasswordHash", "SenhaHash"),
        ("usuarios", "Role", "Perfil"),
        ("filhas", "Name", "Nome"),
        ("filhas", "Education", "Escolaridade"),
        ("semestres", "Year", "Ano"),
        ("semestres", "Half", "Numero"),
        ("disciplinas", "Name", "Nome"),
        ("boletins", "DaughterId", "FilhaId"),
        ("boletins", "SemesterId", "SemestreId"),
        ("notas", "ReportId", "BoletimId"),
        ("notas", "SubjectId", "DisciplinaId"),
        ("notas", "Value", "Valor"),
        ("regras_recompensa", "Minimum", "MediaMinima"),
        ("regras_recompensa", "Amount", "Valor"),
        ("recompensas", "ReportId", "BoletimId"),
        ("recompensas", "Amount", "Valor"),
        ("pagamentos", "RewardId", "RecompensaId"),
        ("pagamentos", "UserId", "UsuarioId"),
        ("pagamentos", "PaidAt", "DataPagamento")
    ];

    public static async Task TraduzirColunasAsync(MySqlConnection conexao)
    {
        foreach (var (tabela, nomeAnterior, nomeAtual) in Alteracoes)
        {
            var colunas = (await conexao.QueryAsync<string>(
                "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME=@tabela",
                new { tabela })).ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (!colunas.Contains(nomeAnterior)) continue;
            if (colunas.Contains(nomeAtual))
                throw new InvalidOperationException($"A tabela {tabela} contém as colunas {nomeAnterior} e {nomeAtual}. Revise a estrutura antes de continuar; nenhum dado foi removido.");

            // Identificadores vêm exclusivamente da lista fixa acima, nunca de entradas do usuário.
            await conexao.ExecuteAsync($"ALTER TABLE `{tabela}` RENAME COLUMN `{nomeAnterior}` TO `{nomeAtual}`");
        }
    }
}
