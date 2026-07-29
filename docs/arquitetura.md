# Arquitetura — OrbitBoard

## Visão geral

O OrbitBoard é uma aplicação full stack composta por três camadas desacopladas, executadas como containers independentes e orquestradas pelo Docker Compose.

```
┌─────────────────────────────────────────────────────────────┐
│                    Máquina do usuário                         │
│                                                               │
│   ┌─────────────┐         requisições HTTP/JSON               │
│   │  Navegador  │──────────────────────┐                      │
│   └─────────────┘                      │                      │
│         │ carrega a SPA                │ fetch para           │
│         │ (HTML/CSS/JS)                │ localhost:5200       │
│         ▼                              ▼                      │
│   ┌──────────────────┐         ┌──────────────────┐           │
│   │  Frontend         │         │  Backend          │          │
│   │  Nginx :5173→:80  │         │  .NET 8 :5200→:8080│         │
│   │  (arquivos        │         │  (API REST +      │          │
│   │   estáticos)      │         │   dados em memória)│         │
│   └──────────────────┘         └──────────────────┘           │
│                                                               │
│                  Rede Docker Compose                          │
└─────────────────────────────────────────────────────────────┘
```

## As três camadas

### 1. Frontend — React 18 + Vite (servido por Nginx)
Single Page Application escrita em React com Vite. No build de produção, o Vite compila a aplicação em arquivos estáticos (HTML, CSS, JS), que são servidos pelo Nginx. O Nginx também resolve o *SPA fallback* (todas as rotas caem em `index.html`, permitindo que o React Router funcione ao recarregar a página) e expõe um endpoint próprio de `/health`.

### 2. Backend — API REST em .NET 8 (ASP.NET Core)
API que expõe os endpoints de projetos, tarefas, membros de equipe e dashboard. Usa controllers, DTOs de request/response, um middleware de tratamento de exceções e documentação interativa via Swagger. O CORS é configurável por variável de ambiente.

### 3. Persistência — Dados em memória
Os dados vivem em memória durante o ciclo de vida do processo da API (não há banco externo). Ao reiniciar o container do backend, os dados retornam ao estado inicial (seed). Essa escolha é intencional e alinhada ao escopo didático do trabalho.

## Fluxo de uma requisição

Exemplo: carregar o dashboard ao abrir a aplicação.

1. O usuário acessa `http://localhost:5173`. O **Nginx** entrega os arquivos estáticos da SPA ao navegador.
2. O JavaScript da SPA executa **no navegador** e dispara um `fetch` para `http://localhost:5200/api/dashboard`.
3. Como é uma requisição cross-origin (portas diferentes: 5173 → 5200), o navegador primeiro envia um **preflight** `OPTIONS`. A API responde `204`, autorizando a origem configurada no CORS.
4. O navegador então faz o `GET` real. A **API .NET** processa, lê os dados em memória e responde `200` com o JSON.
5. A SPA recebe o JSON e renderiza as métricas na tela.

## Decisões de arquitetura relevantes

### Por que a API é chamada pelo host, e não pelo nome do serviço Docker
As requisições partem do **navegador do usuário**, que está fora da rede interna do Docker. Por isso o `VITE_API_URL` aponta para `http://localhost:5200` (a porta publicada no host), e não para `http://backend:8080` (nome de serviço interno, que só é resolvível container-a-container).

### Por que o `VITE_API_URL` é build arg, e não variável de runtime
O Vite substitui `import.meta.env.VITE_API_URL` pelo valor literal **durante o build**. Depois disso, o valor está gravado no JS estático. Por isso o valor é passado como `build.args` no Docker Compose, e não como `environment` do container.

### Por que o CORS é configurável
A origem permitida foi extraída do código e passou a ser lida de configuração (`Cors:AllowedOrigins`, sobrescrita pela variável de ambiente `Cors__AllowedOrigins` no Compose). Isso torna a aplicação portável: a mesma imagem roda em qualquer origem, sem recompilar.

## Mapa de portas

| Serviço | Porta interna (container) | Porta no host |
|---|---|---|
| Frontend (Nginx) | 80 | 5173 |
| Backend (Kestrel/.NET) | 8080 | 5200 |
