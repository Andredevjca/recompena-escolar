using MySqlConnector;

namespace RecompensaEscolar.Infraestrutura;

public class Banco(IConfiguration configuracao, IWebHostEnvironment ambiente)
{
    public bool ModoDemonstracao =>
        ambiente.IsDevelopment() && configuracao.GetValue<bool>("ModoDemonstracao");

    public MySqlConnection CriarConexao()
    {
        var cadeiaConexao = configuracao.GetConnectionString("MySql");
        if (string.IsNullOrWhiteSpace(cadeiaConexao))
            throw new InvalidOperationException("Configure a conexão MySql antes de utilizar o banco.");
        return new MySqlConnection(cadeiaConexao);
    }
}

