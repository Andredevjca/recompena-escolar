using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
using RecompensaEscolar.Servicos;

namespace RecompensaEscolar.Dados;

// Estado e sincronização compartilhados entre os repositórios.
public sealed class EstadoDemonstracao
{
    public SemaphoreSlim Semaforo { get; } = new(1, 1);
    public ModeloPainel Painel { get; } = new();
    public List<UsuarioEscolar> Usuarios { get; } = [];
    public Dictionary<int, List<Nota>> Notas { get; } = [];

    public void Inicializar()
    {
        Usuarios.Add(new(1, "André da Silva Ramos", "admin@admin.com", Senhas.Gerar("admin@admin.com", "admin"), "Administrador"));
        Painel.Filhas = [new(1, "Maria", "Ensino Fundamental"), new(2, "Ana", "Ensino Fundamental")];
        Painel.Semestres = [new(1, 2025, 1), new(2, 2025, 2), new(3, 2026, 1), new(4, 2026, 2)];
        Painel.Regras = [new(1, 0, 0), new(2, 7, 100), new(3, 8, 180), new(4, 9, 250), new(5, 10, 300)];
        decimal[][] medias = [[8.25m, 8.6m, 8.90m, 9.34m], [7.8m, 8.15m, 8.44m, 8.76m]];
        foreach (var filha in Painel.Filhas)
            foreach (var semestre in Painel.Semestres)
            {
                var media = medias[filha.Id - 1][semestre.Id - 1];
                var id = Painel.Boletins.Count + 1;
                Painel.Boletins.Add(new(id, filha.Id, semestre.Id, media, ServicoRecompensas.Calcular(media, Painel.Regras), semestre.Id < 4));
                Notas[id] = [new("Português", media), new("Matemática", media), new("Ciências", media), new("História", media), new("Geografia", media)];
            }
    }
}
