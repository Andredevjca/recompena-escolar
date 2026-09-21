# Recompensa Escolar

Aplicação ASP.NET Core MVC (.NET 10), MySQL e Dapper.

## Executar

Configure ConnectionStrings:MySql em appsettings.json. Para trocar o servidor, edite Server, Port, Database, User e Password na conexão e reinicie a aplicação. O arquivo acompanha a publicação. Variáveis de ambiente têm prioridade.

Execute: dotnet run --launch-profile http

Acesso: http://localhost:5132/Conta/Entrar

## Inicialização do banco

Ao iniciar, a aplicação cria o banco configurado, as tabelas e os relacionamentos ausentes. Um bloqueio no MySQL impede inicializações simultâneas. Atualizações preservam os dados existentes e os quatro semestres por ano.

O administrador admin@admin.com é criado apenas quando não existe, com senha inicial admin ou o valor de SenhaAdministradorInicial. Senhas existentes não são sobrescritas; somente hashes são armazenados.

São inseridas as regras iniciais de recompensa. Filhas, séries, semestres, disciplinas e notas são cadastrados pelo usuário, diretamente no banco.

Requisitos: MySQL 8 ou superior e uma conexão com permissão para criar o banco e atualizar sua estrutura.

## Organização

- Controladores, Servicos e Repositorios: telas, regras e persistência.
- Dados/estrutura.sql: estrutura das tabelas.
- Dados/AtualizadorEstrutura.cs: compatibilidade das colunas e ampliação para quatro semestres.
- Infraestrutura/Banco.cs: conexão MySQL.
- Dados/Chaves: chaves locais dos cookies, ignoradas pelo Git.

O fluxo de inicialização usa C:\controle-alugueis como referência.
