# Roteiro de Apresentação — OrbitBoard

**Duração-alvo:** 8 a 12 minutos
**Formato:** demonstração técnica com divisão de fala entre os integrantes

---

## Estrutura sugerida (ordem de fala)

### 1. Abertura — Leandro (≈1 min)
- Nome do projeto e apresentação dos integrantes.
- Finalidade da aplicação: gestão de projetos, tarefas e equipe (aplicação fornecida — Opção A).
- Objetivo didático: integração full stack, containerização e CI/CD.

### 2. Arquitetura — Leandro (≈1,5 min)
- As três camadas: frontend React/Nginx, backend .NET 8, dados em memória.
- Diagrama de arquitetura (`docs/arquitetura.md`).
- Fluxo de uma requisição: navegador → API → resposta JSON.

### 3. Demonstração da aplicação — José (≈2 min)
- Subir com `docker compose up --build`.
- Mostrar o dashboard com dados reais.
- Criar um projeto, criar uma tarefa, mudar status no quadro.
- Mostrar um erro tratado (criar projeto com nome duplicado → 409).

### 4. API e Swagger — Caio (≈1,5 min)
- Abrir o Swagger em `localhost:5200/swagger`.
- Executar um `GET /api/dashboard` e um `POST` ao vivo.
- Mostrar os códigos de status (200, 201, 409) e o retorno em JSON.

### 5. Docker e Compose — Caio (≈1,5 min)
- Explicar os dois Dockerfiles multi-stage (backend runtime; frontend Nginx).
- Mostrar o `docker-compose.yml`: serviços, portas, `depends_on`.
- Os três ajustes técnicos: CORS configurável, VITE_API_URL via build arg, API pelo host.
- `docker compose ps` e `docker images` (evidência do multi-stage no tamanho das imagens).

### 6. CI/CD — Gabriel (≈1 min)
- Mostrar o workflow do GitHub Actions (`.github/workflows/ci.yml`).
- Abrir a aba **Actions** e mostrar os checks verdes.
- Explicar que build de backend e frontend rodam a cada push/PR.

### 7. Integração e comunicação HTTP — João (≈1 min)
- DevTools → Network: mostrar as chamadas com `200` e o preflight CORS (`204`).
- Reforçar por que a API é chamada pelo host e não pelo nome do serviço.

### 8. Testes, dificuldades e encerramento — José + todos (≈1,5 min)
- Roteiro de testes e evidências (`docs/evidencias-testes.md`).
- Dificuldade real resolvida: o healthcheck que deixava o backend "unhealthy" e a correção com `depends_on` de ordem.
- Contribuição de cada integrante (breve, cada um fala a sua).

---

## Divisão de fala (resumo)

| Integrante | Bloco |
|---|---|
| **Leandro** | Abertura + Arquitetura |
| **José** | Demonstração da aplicação + Testes/encerramento |
| **Caio** | API/Swagger + Docker/Compose |
| **Gabriel** | CI/CD |
| **João** | Integração e comunicação HTTP (DevTools) |

---

## Checklist antes de apresentar

- [ ] `docker compose up --build` testado e funcionando na máquina que vai apresentar
- [ ] Dashboard carregando com dados
- [ ] Swagger acessível
- [ ] Aba Actions do GitHub com checks verdes visível
- [ ] Prints de backup em `docs/evidencias-testes.md` (caso a demo ao vivo falhe)
- [ ] Cada integrante sabe qual é o seu bloco
- [ ] Tempo total ensaiado entre 8 e 12 minutos

> **Dica:** tenha os prints das evidências abertos como plano B. Se a demonstração ao vivo travar (rede, Docker), a apresentação continua com as evidências já capturadas.
