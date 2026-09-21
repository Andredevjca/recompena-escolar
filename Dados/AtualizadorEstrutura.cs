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

    public static async Task AmpliarSemestresAsync(MySqlConnection conexao)
    {
        var restricoes = await conexao.QueryAsync<RestricaoSemestre>("""
            SELECT c.CONSTRAINT_NAME Nome, c.CHECK_CLAUSE Clausula
            FROM information_schema.CHECK_CONSTRAINTS c
            JOIN information_schema.TABLE_CONSTRAINTS t
              ON t.CONSTRAINT_SCHEMA=c.CONSTRAINT_SCHEMA AND t.CONSTRAINT_NAME=c.CONSTRAINT_NAME
            WHERE t.TABLE_SCHEMA=DATABASE() AND t.TABLE_NAME='semestres' AND t.CONSTRAINT_TYPE='CHECK'
            """);
        foreach (var restricao in restricoes)
        {
            var clausula = System.Text.RegularExpressions.Regex.Replace(restricao.Clausula, @"[\s`()]", "").ToLowerInvariant();
            if (clausula != "numeroin1,2") continue;
            // Identificador obtido dos metadados e escapado; troca atômica sem remover registros.
            var nome = restricao.Nome.Replace("`", "``");
            await conexao.ExecuteAsync($"ALTER TABLE semestres DROP CHECK `{nome}`, ADD CONSTRAINT `{nome}` CHECK (Numero IN (1,2,3,4))");
        }
    }

    private sealed class RestricaoSemestre
    {
        public string Nome { get; set; } = "";
        public string Clausula { get; set; } = "";
    }

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
