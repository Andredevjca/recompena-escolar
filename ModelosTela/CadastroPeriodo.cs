using System.ComponentModel.DataAnnotations;
using RecompensaEscolar.Modelos;
namespace RecompensaEscolar.ModelosTela;
public class CadastroPeriodo {
 [Range(1,int.MaxValue)] public int FilhaId { get; set; }
 [Required, StringLength(100)] public string Serie { get; set; } = "";
 [Range(2020,2100)] public int Ano { get; set; } = DateTime.Today.Year;
 public bool Primeiro { get; set; } = true;
 public bool Segundo { get; set; } = true;
 public List<Filha> Filhas { get; set; } = [];
 public List<PeriodoEscolar> Periodos { get; set; } = [];
 public List<Boletim> Boletins { get; set; } = [];
}