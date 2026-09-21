using Dapper;
using RecompensaEscolar.Dados;
using RecompensaEscolar.Infraestrutura;
using RecompensaEscolar.Modelos;
using RecompensaEscolar.ModelosTela;
namespace RecompensaEscolar.Repositorios;
public class RepositorioPeriodos(Banco banco, EstadoDemonstracao estado) {
 public async Task<List<PeriodoEscolar>> ListarAsync() {
  if(banco.ModoDemonstracao) { await estado.Semaforo.WaitAsync(); try { return estado.Periodos.OrderByDescending(x=>x.Ano).ThenBy(x=>x.Numero).ToList(); } finally { estado.Semaforo.Release(); } }
  await using var c=banco.CriarConexao();
  return (await c.QueryAsync<PeriodoEscolar>("SELECT p.FilhaId,p.SemestreId,p.Serie,s.Ano,s.Numero FROM periodos_escolares p JOIN semestres s ON s.Id=p.SemestreId ORDER BY s.Ano DESC,s.Numero")).ToList();
 }
 public async Task CadastrarAsync(CadastroPeriodo dados) {
  var numeros=new[]{dados.Primeiro?1:0,dados.Segundo?2:0}.Where(x=>x>0).ToArray();
  if(numeros.Length==0 || string.IsNullOrWhiteSpace(dados.Serie)) throw new InvalidOperationException("Informe a série e selecione pelo menos um semestre.");
  await estado.Semaforo.WaitAsync();
  try {
   if(banco.ModoDemonstracao) {
    if(!estado.Painel.Filhas.Any(x=>x.Id==dados.FilhaId)) throw new InvalidOperationException("Filha não encontrada.");
    if(estado.Periodos.Any(x=>x.FilhaId==dados.FilhaId && x.Ano==dados.Ano && x.Serie!=dados.Serie.Trim())) throw new InvalidOperationException("Já existe outra série cadastrada para esta filha neste ano.");
    foreach(var numero in numeros) {
     var s=estado.Painel.Semestres.FirstOrDefault(x=>x.Ano==dados.Ano && x.Numero==numero);
     if(s==null) { s=new(estado.Painel.Semestres.Select(x=>x.Id).DefaultIfEmpty().Max()+1,dados.Ano,numero); estado.Painel.Semestres.Add(s); }
     if(!estado.Periodos.Any(x=>x.FilhaId==dados.FilhaId && x.SemestreId==s.Id)) estado.Periodos.Add(new(dados.FilhaId,s.Id,dados.Serie.Trim(),dados.Ano,numero));
    }
    estado.Painel.Semestres=estado.Painel.Semestres.OrderBy(x=>x.Ano).ThenBy(x=>x.Numero).ToList(); return;
   }
   await using var c=banco.CriarConexao(); await c.OpenAsync(); await using var t=await c.BeginTransactionAsync();
   if(await c.QuerySingleOrDefaultAsync<int?>("SELECT Id FROM filhas WHERE Id=@FilhaId FOR UPDATE",dados,t)==null) throw new InvalidOperationException("Filha não encontrada.");
   var series=await c.QueryAsync<string>("SELECT p.Serie FROM periodos_escolares p JOIN semestres s ON s.Id=p.SemestreId WHERE p.FilhaId=@FilhaId AND s.Ano=@Ano",dados,t);
   if(series.Any(x=>x!=dados.Serie.Trim())) throw new InvalidOperationException("Já existe outra série cadastrada para esta filha neste ano.");
   foreach(var numero in numeros) {
    await c.ExecuteAsync("INSERT IGNORE INTO semestres (Ano,Numero) VALUES (@Ano,@numero)",new{dados.Ano,numero},t);
    await c.ExecuteAsync("INSERT IGNORE INTO periodos_escolares (FilhaId,SemestreId,Serie) SELECT @FilhaId,Id,@Serie FROM semestres WHERE Ano=@Ano AND Numero=@numero",new{dados.FilhaId,dados.Ano,numero,Serie=dados.Serie.Trim()},t);
   }
   await t.CommitAsync();
  } finally { estado.Semaforo.Release(); }
 }
}