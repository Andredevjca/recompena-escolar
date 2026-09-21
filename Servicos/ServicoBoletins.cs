using System.Globalization;
using RecompensaEscolar.Interfaces.Repositorios;
using RecompensaEscolar.Interfaces.Servicos;
using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;

namespace RecompensaEscolar.Servicos;

public class ServicoBoletins(IRepositorioBoletins repositorio, RecompensaEscolar.Repositorios.RepositorioPeriodos periodos) : IServicoBoletins
{
    public async Task<List<Filha>> ListarFilhasAsync() => (await repositorio.ObterPainelAsync()).Filhas;

    public async Task CadastrarAsync(CadastroBoletim dados)
    {
        var periodo = (await periodos.ListarAsync()).FirstOrDefault(x => x.FilhaId == dados.FilhaId && x.SemestreId == dados.SemestreId)
            ?? throw new InvalidOperationException("Selecione um semestre cadastrado para esta filha.");
        dados.Ano = periodo.Ano; dados.Numero = periodo.Numero;
        if (dados.Ano is < 2020 or > 2100 || dados.Numero is < 1 or > 2)
            throw new InvalidOperationException("Informe um ano entre 2020 e 2100 e um semestre válido.");

        if (dados.Disciplinas.Length is 0 or > 30 || dados.Disciplinas.Length != dados.Valores.Length)
            throw new InvalidOperationException("Inclua entre 1 e 30 disciplinas com suas notas.");

        var notas = new List<Nota>();
        for (var indice = 0; indice < dados.Disciplinas.Length; indice++)
        {
            var disciplina = dados.Disciplinas[indice]?.Trim() ?? "";
            if (disciplina.Length is < 2 or > 100 ||
                !decimal.TryParse(dados.Valores[indice]?.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var valor) ||
                valor is < 0 or > 10)
                throw new InvalidOperationException($"Confira a disciplina e a nota da linha {indice + 1}. Use notas de 0 a 10.");
            notas.Add(new Nota(disciplina, valor));
        }

        if (notas.Select(nota => nota.Disciplina).Distinct(StringComparer.OrdinalIgnoreCase).Count() != notas.Count)
            throw new InvalidOperationException("Não repita disciplinas no mesmo boletim.");

        var painel = await repositorio.ObterPainelAsync();
        if (!painel.Filhas.Any(filha => filha.Id == dados.FilhaId))
            throw new InvalidOperationException("Selecione uma filha cadastrada.");

        await repositorio.CadastrarBoletimAsync(dados, notas);
    }
}

