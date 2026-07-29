# ============================================================
# ESTÁGIO 1 — build: compila o React/Vite em arquivos estáticos
# Usa a imagem do Node só para BUILDAR. Ela não vai para produção.
# ============================================================
FROM node:20-alpine AS build
WORKDIR /app

# A URL da API é definida em tempo de BUILD (não de execução).
# O Vite "assa" esse valor dentro do JS durante o npm run build.
# Por isso é ARG (argumento de build), não ENV de runtime.
# O padrão aqui vale para build local; o Compose vai sobrescrever.
ARG VITE_API_URL=http://localhost:5200
ENV VITE_API_URL=$VITE_API_URL

# Copia package.json e lockfile primeiro, para aproveitar o cache.
# Enquanto as dependências não mudarem, o npm ci não roda de novo.
COPY package.json package-lock.json ./
RUN npm ci

# Copia o restante do código-fonte e gera o build de produção.
# O resultado sai na pasta /app/dist (padrão do Vite).
COPY . .
RUN npm run build

# ============================================================
# ESTÁGIO 2 — runtime: serve os estáticos com Nginx
# Imagem minúscula. Não tem Node, não tem código-fonte:
# só o Nginx e os arquivos já compilados.
# ============================================================
FROM nginx:alpine AS final

# Substitui a config padrão do Nginx pela nossa.
# O nginx.conf do projeto já resolve o SPA fallback e um /health.
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Traz APENAS os arquivos estáticos gerados no estágio de build.
COPY --from=build /app/dist /usr/share/nginx/html

# O Nginx escuta na porta 80 dentro do container.
EXPOSE 80

# A imagem base do Nginx já sobe o servidor sozinha,
# então não precisamos declarar ENTRYPOINT/CMD.