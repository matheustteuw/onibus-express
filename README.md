# OniBus Express

<img width="942" height="449" alt="image" src="https://github.com/user-attachments/assets/c178ee3e-eb44-449a-bec9-39e39e466bf4" />

Sistema de venda de passagens rodoviárias (desafio técnico). Permite buscar rotas e viagens, reservar assento, consultar e cancelar reserva.

Backend em .NET e frontend em React, no mesmo repositório.

## Como rodar

### Backend, com Docker (recomendado)

Pré-requisito: Docker Desktop instalado e rodando.

```bash
docker-compose up --build
```

Sobe dois containers: `db` (SQL Server 2022) e `api`. A API espera o banco ficar saudável (healthcheck), aplica as migrations e popula rotas/viagens de exemplo automaticamente ao iniciar — não precisa rodar nenhum comando extra.

- API: http://localhost:8080
- Swagger: http://localhost:8080/swagger

### Backend, sem Docker

Pré-requisitos: .NET 8 SDK e SQL Server (ou LocalDB, que já vem com o Visual Studio).

```bash
# instalar a ferramenta de migrations do EF Core, se ainda não tiver
dotnet tool install --global dotnet-ef

# aplicar as migrations no banco configurado em appsettings.json
dotnet ef database update --project src/Backend/OniBusExpress.Infrastructure --startup-project src/Backend/OniBusExpress.API

# rodar a API
dotnet run --project src/Backend/OniBusExpress.API
```

A connection string padrão em `appsettings.json` aponta para `(localdb)\MSSQLLocalDB`. Troque para o seu SQL Server local se preferir.

### Frontend

Pré-requisito: Node 20+.

```bash
cd src/Frontend/onibus-express-web
npm install
npm run dev
```

- App: http://localhost:5173

O frontend consome a API via a variável `VITE_API_BASE_URL` (arquivo `.env`, já configurada para `http://localhost:8080`). Precisa da API rodando (com ou sem Docker) para funcionar de verdade.

Empacotamento em Docker (Nginx) ainda não foi feito — ver "O que ficou de fora".

## Tecnologias

### Backend

- **.NET 8 / ASP.NET Core Web API** — exigência do desafio.
- **EF Core 8 + SQL Server** — ORM e banco relacional, com migrations versionadas no repositório.
- **AutoMapper** — mapeamento entre entidades de domínio e os DTOs de request/response, evitando código repetitivo de conversão.
- **FluentValidation** — validação dos dados de entrada (CPF, campos obrigatórios) de forma declarativa, separada da lógica de negócio.
- **Swashbuckle (Swagger)** — documentação e exploração interativa dos endpoints.
- **xUnit + Moq + FluentAssertions** — testes unitários. Moq isola os use cases dos repositórios reais; FluentAssertions deixa as asserções mais legíveis.
- **Docker + docker-compose** — sobe API e banco com um único comando.

### Frontend

- **React 19 + TypeScript**, com **Vite** — bundler mais rápido e mais simples de configurar que Create React App (descontinuado) para uma SPA sem necessidade de SSR.
- **React Router** — navegação entre as telas do fluxo de compra.
- **Zustand** — guarda a viagem, o assento e o código da reserva enquanto o usuário navega entre as telas. Mais direto que Context API + reducer para esse volume de estado compartilhado.
- **CSS Modules** — estilos isolados por componente, sem dependência de biblioteca de UI.
- **Vitest + React Testing Library** — mesmo runner do bundler (Vite), evita configuração extra de transform que o Jest exigiria.

## Arquitetura

```
src/
  Backend/
    OniBusExpress.API             # controllers, filtro de exceção, Program.cs
    OniBusExpress.Application     # use cases (1 classe por ação), validators, AutoMapper
    OniBusExpress.Domain          # entidades, interfaces de repositório, regras que não dependem de infra
    OniBusExpress.Infrastructure  # EF Core, migrations, implementação dos repositórios
  Shared/
    OniBusExpress.Communication   # Requests/Responses (contrato da API)
    OniBusExpress.Exceptions      # exceções de domínio e mensagens de erro
  Frontend/
    onibus-express-web/
      src/pages                   # uma tela por rota
      src/components              # TripCard, SeatMap, PassengerForm
      src/services                # cliente HTTP e chamadas à API
      src/store                   # estado compartilhado do fluxo de compra (Zustand)
      src/types                   # tipos espelhando os DTOs do backend
tests/
  OniBusExpress.Tests             # testes unitários do backend
```

### Decisões do backend

- **Um use case por ação** (`RegisterReservationUseCase`, `CancelReservationUseCase`, etc.), cada um com sua interface. Mantém a lógica de negócio isolada dos controllers e fácil de testar sem subir a API inteira.
- **Repositórios segregados por responsabilidade** (`IReservationReadOnlyRepository`, `IReservationWriteOnlyRepository`, `IReservationUpdateOnlyRepository`) em vez de um repositório genérico único. Deixa explícito o que cada use case realmente precisa fazer com os dados.
- **Sem coluna de "assentos disponíveis"** na tabela de viagens. Esse número é sempre calculado a partir de `TotalSeats` menos as reservas ativas, evitando um contador que pode dessincronizar do estado real.
- **Dois índices únicos filtrados no banco** (`WHERE Status = 1`), não só validação em código:
  - `(TripId, SeatNumber)` — garante que não existam duas reservas ativas no mesmo assento, mesmo sob concorrência (duas requisições simultâneas tentando o mesmo assento).
  - `(TripId, PassengerId)` — impede que a mesma pessoa (mesmo CPF) fique com duas reservas ativas na mesma viagem, já que fisicamente ela só pode ocupar um assento por vez. Essa regra não está no enunciado do desafio, foi uma extensão que fez sentido adicionar.
- **Filtro global de exceções** (`ExceptionFilter`) que converte as exceções de domínio (`NotFoundException`, `ConflictException`, `ErrorOnValidationException`) no status HTTP correto (404, 409, 400), em vez de cada controller tratar isso manualmente.
- **CPF armazenado sem máscara** (só os 11 dígitos), pra não ter o mesmo CPF gravado de duas formas diferentes no banco.
- **CORS liberado** para `localhost:5173` (dev do Vite) e `localhost:3000` (porta prevista para o frontend em Docker), para o navegador poder chamar a API rodando em outra porta.

### Decisões do frontend

- **Fluxo guiado por rotas** (`/`, `/viagens/:id/assentos`, `/viagens/:id/passageiro`, `/confirmacao`, `/reservas/consulta`) em vez de um wizard controlado só por estado interno. Cada tela é uma URL navegável.
- **Validação de CPF duplicada no frontend** (mesmo algoritmo do backend, em `utils/cpf.ts`). O backend continua sendo a fonte da verdade — a validação no frontend é só para dar feedback imediato ao usuário, sem esperar o round-trip da API.
- Telas que dependem de estado do fluxo (seleção de assento, dados do passageiro, confirmação) redirecionam para a busca se acessadas diretamente sem uma viagem selecionada.

## O que foi implementado

Backend:

- Os 6 endpoints pedidos: `GET /rotas`, `GET /viagens`, `GET /viagens/{id}`, `POST /reservas`, `GET /reservas/{codigo}`, `DELETE /reservas/{codigo}`.
- Todas as regras de negócio do desafio: assento ocupado, viagem já realizada, validação de CPF com dígito verificador, código de reserva único e legível (`ABC-12345`), cancelamento até 2h antes da partida.
- Regra extra: um mesmo passageiro não pode ter duas reservas ativas na mesma viagem.
- Seed automático de rotas e viagens ao iniciar a aplicação (banco vazio), para dar dados de teste sem precisar de um endpoint de cadastro de rota/viagem (o desafio não pede isso).
- Testes unitários dos 4 pontos pedidos no desafio.
- Docker Compose com API + SQL Server + migration automática.

Frontend:

- Tela 1 — busca de passagens (origem, destino, data), com loading e mensagem de "nenhum resultado".
- Tela 2 — seleção de assento, com mapa visual (livre/ocupado/selecionado) e bloqueio de assentos já ocupados.
- Tela 3 — dados do passageiro com validação (nome, CPF, e-mail), resumo da compra e tela de sucesso com o código da reserva.
- Tela 4 (bônus) — consulta de reserva por código, com opção de cancelar.

## O que ficou de fora

- Autenticação/autorização (não fazia parte do escopo pedido).
- Testes de integração com banco real (SQLite in-memory ou TestContainers) no backend. Optei por testes unitários com repositórios mockados, que já cobrem os 4 pontos pedidos pelo desafio.
- **Testes do frontend** (componente de busca, mapa de assentos, validação do formulário) — ainda não escritos.
- **Docker do frontend** (Nginx servindo o build) e a integração dele no `docker-compose.yml` — ainda não feito. Hoje o frontend só roda via `npm run dev`.
- Cadastro de rotas/viagens via API (o desafio só pede leitura desses recursos — populei via seed).
- Paginação em `GET /viagens` (hoje retorna todos os resultados da busca).

## Como rodar os testes

Backend:

```bash
dotnet test
```

Cobre: validação de CPF (formato e dígito verificador), regra de assento já ocupado, regra de cancelamento dentro do prazo, e geração de código de reserva único (incluindo o retry quando o código gerado já existe).

Frontend: ainda não implementado (ver "O que ficou de fora").

## Endpoints

Documentação interativa via Swagger em `/swagger` (só ativo em ambiente Development, que é o padrão tanto local quanto no Docker Compose).

| Método | Rota | Descrição |
|---|---|---|
| GET | `/rotas` | Lista todas as rotas |
| GET | `/viagens?origin=&destination=&departureDate=` | Busca viagens por origem, destino e data |
| GET | `/viagens/{id}` | Detalhes de uma viagem, com assentos livres/ocupados |
| POST | `/reservas` | Cria uma reserva |
| GET | `/reservas/{codigo}` | Consulta uma reserva pelo código |
| DELETE | `/reservas/{codigo}` | Cancela uma reserva |

## Pontos de melhoria com mais tempo

- Testes do frontend (busca, mapa de assentos, validação do formulário) e testes de integração do backend com banco real.
- Docker do frontend e `docker-compose.yml` unificado subindo API + banco + frontend com um único comando.
- Paginação e ordenação em `GET /viagens`.
- Endpoint de cadastro de rotas/viagens, caso o sistema precise ser administrado sem acesso direto ao banco.
- Observabilidade (logging estruturado, health check endpoint dedicado).
