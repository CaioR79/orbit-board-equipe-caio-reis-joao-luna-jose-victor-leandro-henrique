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

> **Nota de Comunicação:** Toda a comunicação é feita no lado do cliente. O navegador renderiza o frontend e faz as requisições HTTP para os endpoints da API utilizando a porta exposta diretamente no host local.

## Tecnologias
 - **Frontend:** React 18, Vite, Nginx
 - **Backend:** .NET 8, ASP.NET Core, Swagger
 - **Infra:** Docker, Docker Compose
 - **CI:** GitHub Actions

## Como executar

### Via Docker Compose (recomendado)
 Para subir todo o ambiente de forma integrada (Frontend, API e configurações de proxy/portas), execute na raiz do projeto:
 ```bash
 docker compose up --build
 ```

### Localmente (sem Docker)

Para rodar o projeto em sua máquina local sem a necessidade de containers, siga as etapas abaixo:

#### 1. Configurando e executando o Backend (.NET 8)
 Navegue até o diretório do backend, restaure as dependências e inicie a API:
 ```bash
 cd backend
 dotnet run
 ```
 A API estará ativa em `http://localhost:5200`.

#### 2. Configurando e executando o Frontend (React + Vite)
 Em um novo terminal, navegue até a pasta do frontend, instale os pacotes necessários e inicie o servidor de desenvolvimento:
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
 * `GET /api/dashboard` — Retorna os dados consolidados, métricas e visão geral para o painel de controle principal.
 * `GET/POST/PUT/DELETE /api/projects` — Endpoints para criação, listagem, atualização e exclusão de projetos.
 * `GET/POST/PUT/DELETE /api/tasks` — Endpoints para gerenciamento do ciclo de vida das tarefas do projeto.
 * `GET/POST/PUT/DELETE /api/team-members` — Endpoints para cadastro e atribuição de membros de equipe.
 * `GET /health` — Endpoint que retorna o status de integridade (Health Check) da API.

## Variáveis de ambiente
 As configurações de conexões e portas do projeto são parametrizadas usando como base o arquivo `.env.example`. Certifique-se de configurar:
 * `VITE_API_URL`: Define o endereço base da API a ser consumido pelo frontend (ex: `http://localhost:5200`).
 * `Cors__AllowedOrigins`: Configura as origens permitidas pelo CORS na API .NET para autorizar as requisições do frontend (ex: `http://localhost:5173`).
 * **Portas**: Configurações de portas locais para o mapeamento dos containers no Docker Compose.

## Evidências
 Toda a documentação complementar do projeto, incluindo diagramas de arquitetura, especificações de contrato de API e relatórios/capturas de tela de testes realizados, estão disponíveis na pasta correspondente:
 * [Acessar a pasta de evidências e documentações](./docs)

## Contribuição da equipe
 * **Caio Reis:** Conduziu a etapa de desenvolvimento e otimização do Frontend com React e Vite.
 * **José Victor:** Liderou a modelagem da API .NET 8 e estruturação dos dados em memória.
 * **Gabriel Vitório:** Responsável pela configuração do Docker, Docker Compose e orquestração dos containers.
 * **João Luna:** Conduziu a implementação do pipeline de CI/CD via GitHub Actions e automação de testes.
 * **Leandro Henrique:** Liderou a elaboração da documentação técnica, design de arquitetura e organização de evidências de entrega.
