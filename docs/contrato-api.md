# Contrato da API — OrbitBoard

Base URL (local/Docker): `http://localhost:5200`
Documentação interativa: `http://localhost:5200/swagger`

Os identificadores (`id`, `ownerId`, `projectId`, `assigneeId`) são **GUIDs**.

## Enumerações

| Enum | Valores possíveis |
|---|---|
| `ProjectStatus` | `Planning`, `Active`, `OnHold`, `Completed` |
| `WorkItemStatus` | `Backlog`, `InProgress`, `Review`, `Done` |
| `WorkItemPriority` | `Low`, `Medium`, `High`, `Critical` |

---

## Health

### `GET /health`
Retorna o status de integridade da API.

**Resposta `200 OK`:**
```json
{ "status": "healthy", "service": "OrbitBoard.Api", "utcTime": "2026-07-29T12:00:00+00:00" }
```

---

## Dashboard

### `GET /api/dashboard`
Retorna as métricas consolidadas do workspace.

**Resposta `200 OK`:**
```json
{
  "totalProjects": 3,
  "activeProjects": 2,
  "totalTasks": 5,
  "completedTasks": 1,
  "overdueTasks": 0,
  "recentTasks": [ ... ]
}
```

---

## Projects

### `GET /api/projects`
Lista todos os projetos. → `200 OK`

### `GET /api/projects/{id}`
Retorna um projeto específico.
- `200 OK` — projeto encontrado
- `404 Not Found` — id inexistente

### `POST /api/projects`
Cria um projeto.

**Payload:**
```json
{
  "name": "Nome do projeto",           // obrigatório, 3–80 caracteres
  "description": "Descrição detalhada", // obrigatório, 10–500 caracteres
  "status": "Planning",                 // opcional (padrão: Planning)
  "startDate": "2026-07-29",            // opcional (padrão: hoje)
  "dueDate": "2026-08-30",              // opcional
  "ownerId": "GUID-do-responsavel"      // obrigatório
}
```

**Respostas:**
- `201 Created` — criado (header `Location` aponta para o recurso)
- `400 Bad Request` — payload inválido (validação de campos)
- `409 Conflict` — já existe projeto com o mesmo nome

### `PUT /api/projects/{id}`
Atualiza um projeto existente. Mesmo payload do `POST` (todos os campos).
- `200 OK` — atualizado
- `404 Not Found` — id inexistente

### `DELETE /api/projects/{id}`
Remove um projeto.
- `204 No Content` — removido
- `409 Conflict` — o projeto ainda possui tarefas associadas

---

## Tasks

### `GET /api/tasks`
Lista tarefas, com filtros opcionais via query string:

| Parâmetro | Tipo | Descrição |
|---|---|---|
| `projectId` | GUID | filtra por projeto |
| `status` | WorkItemStatus | filtra por status |
| `priority` | WorkItemPriority | filtra por prioridade |
| `assigneeId` | GUID | filtra por responsável |
| `search` | string | busca textual |

Exemplo: `GET /api/tasks?status=InProgress&priority=High` → `200 OK`

### `GET /api/tasks/{id}`
- `200 OK` — encontrada
- `404 Not Found` — id inexistente

### `POST /api/tasks`
Cria uma tarefa.

**Payload:**
```json
{
  "projectId": "GUID-do-projeto",       // obrigatório
  "title": "Título da tarefa",          // obrigatório, 3–120 caracteres
  "description": "Descrição",           // obrigatório, 5–800 caracteres
  "status": "Backlog",                  // opcional (padrão: Backlog)
  "priority": "Medium",                 // opcional (padrão: Medium)
  "assigneeId": "GUID-do-responsavel",  // opcional
  "dueDate": "2026-08-15",              // opcional
  "estimatedHours": 8                   // opcional (1–200, padrão: 1)
}
```

**Respostas:**
- `201 Created` — criada
- `400 Bad Request` — payload inválido

### `PUT /api/tasks/{id}`
Atualiza uma tarefa (todos os campos). → `200 OK`

### `PATCH /api/tasks/{id}/status`
Altera apenas o status da tarefa (usado pelo quadro).

**Payload:**
```json
{ "status": "InProgress" }
```
→ `200 OK`

### `DELETE /api/tasks/{id}`
Remove uma tarefa. → `204 No Content`

---

## Team Members

### `GET /api/team-members`
Lista os membros de equipe. → `200 OK`

---

## Tratamento de erros

A API usa um middleware de tratamento de exceções que padroniza as respostas de erro no formato **ProblemDetails** (RFC 7807), retornando os códigos de status apropriados (`400`, `404`, `409`) com uma mensagem descritiva.
