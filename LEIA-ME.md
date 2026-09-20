# Recompensa Escolar

Aplicação ASP.NET Core MVC com MySQL, Dapper, Bootstrap, Font Awesome e Nunito.
As telas e a organização do código usam o projeto `C:\jca-inventario` como referência.
Os conceitos de camadas foram adaptados com nomes em português e acesso ao MySQL.

## Executar

Na pasta do projeto:

```powershell
dotnet run --launch-profile http
```

Acesso: http://localhost:5132/Conta/Entrar

O ambiente de desenvolvimento utiliza `ModoDemonstracao: true`.
Credenciais demonstrativas: `admin@admin.com` / `admin`.
Nesse modo, os dados e a sessão existem somente durante a execução da aplicação.

## Organização e nomenclatura

- `Controladores/`: ações das telas e autenticação.
- `Modelos/`: entidades, cada uma em seu próprio arquivo.
- `ModelosTela/`: dados dos formulários, painel e tela de erro.
- `Repositorios/`: consultas, gravações com Dapper e inicialização da estrutura.
- `Interfaces/Repositorios/` e `Interfaces/Servicos/`: contratos das camadas.
- `Servicos/`: autenticação, validação dos boletins, cálculo de recompensa, senhas e coordenação da inicialização.
- `Infraestrutura/Banco.cs`: criação das conexões MySQL.
- `Dependencias/InjecaoDependencias.cs`: registro centralizado das dependências.
- `Dados/estrutura.sql`: tabelas e relacionamentos em português.
- `Dados/AtualizadorEstrutura.cs`: conversão dos nomes antigos das colunas.
- `Views/<Funcionalidade>/`: telas de cada controller; `Views/Shared/`: componentes compartilhados.
- `wwwroot/css/`, `wwwroot/js/aplicacao.js` e `wwwroot/imagens/`: estilos, comportamentos e imagens.

Classes, propriedades, métodos próprios, variáveis, rotas, seletores e campos do banco usam português, sem acentos nos identificadores.
Palavras-chave das linguagens, nomes das APIs e convenções do ASP.NET, Bootstrap, Font Awesome, HTML e CSS permanecem como definidos pelas bibliotecas.
As pastas `Views` e `Shared`, os arquivos `Program.cs`, `appsettings*.json`, `_ViewImports.cshtml` e `_ViewStart.cshtml` seguem as convenções do ASP.NET.

## MySQL

Configure `ConnectionStrings:MySql` e desative `ModoDemonstracao`.
Como no projeto de referência, configurações particulares podem ficar em `appsettings.Local.json`, arquivo opcional ignorado pelo Git e pela publicação. Variáveis de ambiente têm prioridade.
Exemplo de configuração por variáveis de ambiente:

```powershell
$env:ConnectionStrings__MySql = 'Server=localhost;Database=recompensa_escolar;User ID=seu_usuario;Password=sua_senha'
$env:ModoDemonstracao = 'false'
dotnet run --launch-profile http
```

A inicialização cria o banco e as tabelas ausentes sem apagar registros.
O administrador inicial é criado somente se o e-mail ainda não existir.
A configuração opcional `SenhaAdministradorInicial` define sua senha inicial; o padrão solicitado é `admin`. O banco armazena somente o hash da senha.

Para bancos da primeira versão, o atualizador identifica as colunas em inglês e usa `RENAME COLUMN` para convertê-las, preservando os dados. Essa atualização requer MySQL 8 ou superior e permissão de alteração da estrutura.
Os nomes antigos aparecem somente no mapa de compatibilidade do atualizador.
A conversão não foi executada contra um banco nesta entrega.

Validacao da refatoracao: build sem avisos ou erros; verificacoes HTTP no modo demonstracao de login, nove telas, links internos, cadastro de filha, boletim, duplicidade, regras, pagamento, usuarios, permissoes e antiforgery. As regras do .gitignore foram conferidas com git check-ignore. MySQL nao foi validado nesta refatoracao.


## Camadas por funcionalidade

Cada funcionalidade (Inicio, Filhas, Boletins, Historico, Recompensas, Regras, Usuarios e Conta) possui controller, servico, repositorio e interfaces de servico e repositorio. Os controllers dependem apenas dos contratos de servico. Cadastro e detalhes de filhas ficam em Filhas.

O RepositorioPainel concentra a consulta de leitura compartilhada pelos paineis, sem duplicar SQL. As gravacoes ficam nos repositorios de cada funcionalidade. EstadoDemonstracao preserva os dados em memoria e o mesmo semaforo entre os repositorios; somente esse estado e Banco sao singleton.

As rotas seguem /Funcionalidade/Acao, por exemplo /Filhas/Filhas, /Filhas/NovaFilha, /Boletins/Boletins e /Usuarios/Usuarios. O inicio continua em /Inicio/Inicio.

## Preparar uma copia do projeto

Requisitos: SDK .NET 10 e, para dados persistentes, MySQL 8 ou superior.

```powershell
dotnet restore
dotnet build
dotnet run --launch-profile http
```

Para usar MySQL, copie `appsettings.Local.example.json` para `appsettings.Local.json` e preencha os dados locais. Nao coloque credenciais reais nos arquivos versionados.

O `.gitignore` exclui compilacao, publicacao, caches de IDE, dependencias restauraveis, logs, resultados de testes e configuracoes privadas. Mantenha no Git o `.csproj`, fontes, views, `wwwroot` (incluindo as bibliotecas utilizadas), `Dados/estrutura.sql`, configuracoes sem segredos, perfil de execucao e este guia. Arquivos ja rastreados pelo Git nao deixam de ser rastreados apenas por entrar no `.gitignore`.
