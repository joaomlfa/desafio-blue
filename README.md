# DesafioBlue — Como rodar a aplicação e gerenciar migrations junto com Docker

Este README descreve os passos mínimos para executar a aplicação ASP.NET Core, criar e aplicar migrations para o `ContactContext` (PostgreSQL) no projeto e rodar script para criação da imagem com docker.

## Adendo

Toda a aplicação foi feita usando um computador com Linux(Fedora) alguns comandos eu deixei conforme o ambiente, alguns talves necessitem de alguns ajustes para rodar em outras plataformar como o Windows.

## Pré-requisitos

- .NET SDK 10.x instalado (dotnet) — verifique com `dotnet --version`.
- PostgreSQL rodando localmente (ou outro servidor acessível).
- (Opcional) VS Code com extensão C# para depuração.

## Arquivos/Componentes importantes

- `Program.cs` — registro dos serviços e do `ContactContext`.
- `Infrastructure/Persistence/Context/ContactContext.cs` — definição do `DbContext`.
- `Infrastructure/Persistence/Context/ContactContextFactory.cs` — fábrica design-time (gerada para permitir `dotnet ef` sem carregar o Host).
- `.vscode/launch.json` e `.vscode/tasks.json` — configuração de debug no VS Code (opcional).

## Configurar connection string

Você pode fornecer a connection string de duas maneiras principais:

1. Variável de ambiente (recomendado para uso local com as ferramentas EF de design-time)

```bash
export CONNECTION_STRING="Host=localhost;Database=desafioblue;Username=postgres;Password=postgres"
```

O `ContactContextFactory` (arquivo em `Infrastructure/Persistence/Context/ContactContextFactory.cs`) irá usar `CONNECTION_STRING` quando as ferramentas `dotnet ef` precisarem instanciar o contexto em tempo de design. Se a variável não existir, a factory usa um fallback embutido.

2. Configuração em `appsettings.Development.json`/`appsettings.json`

Se preferir que a aplicação use a configuração do `appsettings`, registre o `ContactContext` em `Program.cs` lendo `builder.Configuration.GetConnectionString(...)` e garantindo que a mesma connection string seja visível ao processo (por exemplo, definindo `ASPNETCORE_ENVIRONMENT=Development` para usar `appsettings.Development.json`).

## Instalar/atualizar ferramentas (dotnet-ef)

Instale ou atualize a ferramenta global `dotnet-ef` para correspondência com a versão do runtime (.NET 10):

```bash
# instalar (se não instalado)
dotnet tool install --global dotnet-ef

# ou atualizar para versão compatível
dotnet tool update --global dotnet-ef
```

Verifique a versão:

```bash
dotnet ef --version
```

> Nota: se você preferir executar `dotnet ef` como ferramenta local, use as diretivas de tool manifest em vez do `--global`.

## Criar uma migration (ContactContext)

No diretório do projeto (`/home/joaomlfa/desafio-blue/DesafioBlue`) rode:

```bash
# criar migration chamada InitialCreate
dotnet ef migrations add InitialCreate \
  -c ContactContext \
  -o Infrastructure/Persistence/Migrations/Contact
```

Opções usadas:

- `-c ContactContext`: indica o DbContext alvo.
- `-o ...`: pasta de saída das migrations (relativa ao projeto).

Se você tiver um projeto separado de startup, acrescente `--project` e `--startup-project` conforme necessário.

## Aplicar as migrations (criar/atualizar o banco)

```bash
dotnet ef database update -c ContactContext
```

Esse comando vai aplicar todas as migrations pendentes ao banco apontado pela `ContactContextFactory` (ou pela configuração da aplicação, caso você tenha feito a factory para ler de `appsettings`).

## Reverter/remover uma migration

```bash
# remover última migration (antes de aplicar)
dotnet ef migrations remove -c ContactContext

# voltar o banco para um migration anterior
dotnet ef database update NomeDaMigrationAnterior -c ContactContext
```

## Rodar a aplicação

Via linha de comando:

```bash
dotnet run
```

No VS Code:

1. Abra a pasta do projeto no VS Code.
2. Abra o painel Run and Debug (Ctrl+Shift+D).
3. Selecione a configuração ".NET Launch (console)" e pressione F5.

A configuração de debug criada usa a task `build` e aponta o `program` para o arquivo `bin/Debug/net10.0/DesafioBlue.dll`.

## Acessar Swagger (OpenAPI)

Se a aplicação estiver rodando (em Development), a Swagger UI fica disponível em:

- http://localhost:7125/swagger/index.html (porta pode variar)

## Testes

O projeto inclui uma suíte de testes xUnit em `DesafioBlue.Tests` com testes unitários e de integração leve (InMemory) para handlers, repositórios, validators e controllers.

Comandos úteis:

```bash
# ir para a pasta de testes
cd /home/joaomlfa/desafio-blue/DesafioBlue.Tests

# restaurar dependências (opcional)
dotnet restore

# executar todos os testes
dotnet test

# executar testes sem rebuild (quando já estiver compilado)
dotnet test --no-build

# filtrar por nome de teste (ex.: validators)
dotnet test --filter "FullyQualifiedName~ValidatorTests"
```

## Docker

Para facilitar a execução
`Dockerfile` em `DesafioBlue/` e um `docker-compose.yml` na raiz do repositório. O Compose sobe dois serviços:

- `db` — Postgres (porta 5432)
- `app` — aplicação ASP.NET rodando em um container (mapeado para a porta 5000 do host)

Passos rápidos:

1. Construa e suba os containers com Docker Compose:

```bash
cd /home/joaomlfa/desafio-blue
docker compose up --build
```

2. A aplicação ficará disponível em http://localhost:5000 (Swagger em `/swagger/index.html`).

3. Variáveis importantes (definidas no `docker-compose.yml`):

- `ConnectionStrings__DefaultConnection` — connection string usada pela aplicação para conectar ao `db`.
- `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB` — configuram o banco Postgres.

Observações e troubleshooting:

- O `Program.cs` chama `db.Database.Migrate()` em startup; se o container da aplicação tentar migrar antes do Postgres estar pronto, o Compose depende_do (`depends_on`) ajuda, mas em alguns ambientes pode ser necessário um healthcheck/wait-for script.
- Para parar e remover containers, volumes e rede gerada:

```bash
docker compose down -v
```

### Atalho: script de conveniência

Um pequeno script helper foi criado em `scripts/start-docker.sh` para facilitar comandos comuns do Docker Compose (up, rebuild, down, logs, status).

Exemplo de uso:

```bash
# subir containers (build se necessário)
./scripts/start-docker.sh up

# rebuildar a imagem do app e subir
./scripts/start-docker.sh rebuild

# parar e remover containers/volumes
./scripts/start-docker.sh down

# ver logs do app
./scripts/start-docker.sh logs

# verificar status (compose ps)
./scripts/start-docker.sh status
```

O script já foi marcado como executável no repositório; se necessário, torne-o executável localmente com `chmod +x scripts/start-docker.sh`.

## Frontend

Na pasta frontend possui o sistema em vue js, basta entrar na mesma e rodar com o comando.

```bash
npm run serve
```

o front ja esta com apontamento correto para api rodando localmente e a api ja libera o CORS para o front local.
