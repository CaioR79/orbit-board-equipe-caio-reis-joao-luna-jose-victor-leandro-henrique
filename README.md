# OrbitBoard — Integração Full Stack

Projeto final do Módulo 5. Aplicação de gestão de projetos, tarefas e equipe.

## Integrantes
- Caio Reis
- João Luna
- José Victor
- Leandro Henrique
- Gabriel Vitório

## Descrição
O OrbitBoard é uma aplicação web voltada para a gestão centralizada de projetos, controle de tarefas e organização de membros de equipes. Desenvolvido como o projeto final do Módulo 5, o trabalho tem como objetivo didático consolidar práticas de desenvolvimento de ponta a ponta (Full Stack), integrando a componentização do React com a robustez de uma API em .NET 8, além de sedimentar conhecimentos em containerização e fluxos de CI/CD.

## Arquitetura
A arquitetura do projeto segue um modelo desacoplado composto por três camadas fundamentais:
* **Frontend:** Uma Single Page Application (SPA) desenvolvida em React 18 (utilizando Vite). Em ambiente de produção (via Docker), ela é empacotada e servida de forma otimizada por um servidor web **Nginx**.
* **Backend (API):** Uma API REST estruturada em **.NET 8 (ASP.NET Core)**, responsável pela lógica de negócios, roteamento e geração automatizada de documentação interativa através do Swagger.
* **Persistência de Dados:** Visando a simplicidade e a agilidade didática do projeto, os dados são gerenciados **em memória (In-Memory)** durante o ciclo de vida da API, dispensando a necessidade de infraestrutura de banco de dados externa.

> **Nota de Comunicação:** Toda a comunicação parte do lado do cliente. O navegador renderiza o frontend e faz as requisições HTTP para os endpoints da API utilizando a porta exposta diretamente no host local (não o nome do serviço interno do Docker).

Para detalhes aprofundados, consulte [`docs/arquitetura.md`](./docs/arquitetura.md).

## Tecnologias
- **Frontend:** React 18, Vite, Nginx
- **Backend:** .NET 8, ASP.NET Core, Swagger
- **Infra:** Docker, Docker Compose
- **CI:** GitHub Actions

## Como executar

### Via Docker Compose (recomendado)
Para subir todo o ambiente de forma integrada (frontend, API e configurações de portas), execute na raiz do projeto:
```bash
docker compose up --build
```

### Localmente (sem Docker)

#### 1. Backend (.NET 8)
Navegue até o diretório do backend e inicie a API:
```bash
cd backend
dotnet run --project OrbitBoard.Api
```
A API estará ativa em `http://localhost:5200`.

#### 2. Frontend (React + Vite)
Em um novo terminal, navegue até a pasta do frontend, instale os pacotes e inicie o servidor de desenvolvimento:
```bash
cd frontend
npm install
npm run dev
```
O frontend estará acessível em `http://localhost:5173`.

## URLs de acesso
| Serviço | URL |
|---|---|
| Frontend | http://localhost:5173 |
| API | http://localhost:5200 |
| Swagger | http://localhost:5200/swagger |
| Health (API) | http://localhost:5200/health |

## Endpoints principais
* `GET /api/dashboard` — Retorna os dados consolidados, métricas e visão geral para o painel de controle.
* `GET/POST/PUT/DELETE /api/projects` — Criação, listagem, atualização e exclusão de projetos.
* `GET/POST/PUT/PATCH/DELETE /api/tasks` — Gerenciamento do ciclo de vida das tarefas (inclui alteração de status via `PATCH`).
* `GET /api/team-members` — Listagem dos membros de equipe.
* `GET /health` — Status de integridade (health check) da API.

O contrato detalhado (payloads e códigos de status) está em [`docs/contrato-api.md`](./docs/contrato-api.md).

## Variáveis de ambiente
As configurações de conexão e portas são parametrizadas com base no arquivo `.env.example`:
* `VITE_API_URL`: endereço base da API consumido pelo frontend (ex: `http://localhost:5200`). Resolvido em **tempo de build** pelo Vite.
* `Cors__AllowedOrigins`: origens permitidas pelo CORS na API .NET (ex: `http://localhost:5173`).
* **Portas:** mapeamento dos containers no Docker Compose (5200 → API, 5173 → frontend).

## Evidências
Documentação complementar — diagramas de arquitetura, contrato de API e relatórios/capturas de tela dos testes — na pasta [`docs/`](./docs):
* [`arquitetura.md`](./docs/arquitetura.md)
* [`contrato-api.md`](./docs/contrato-api.md)
* [`evidencias-testes.md`](./docs/evidencias-testes.md)
* [`roteiro-apresentacao.md`](./docs/roteiro-apresentacao.md)

## Contribuição da equipe
* **Caio Reis:** Containerização do backend (.NET 8), orquestração via Docker Compose e ajustes técnicos de integração (CORS parametrizável e build args do Vite).
* **João Luna:** Containerização do frontend (React/Vite + Nginx) e revisão técnica da integração.
* **José Victor:** Testes de integração, roteiro de validação e coleta de evidências de funcionamento.
* **Gabriel Vitório:** Pipeline de CI/CD com GitHub Actions (build automatizado de backend e frontend).
* **Leandro Henrique:** Documentação técnica, diagrama de arquitetura e organização das evidências de entrega.
