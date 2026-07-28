# ============================================================
# ESTÁGIO 1 — build: compila e publica a aplicação
# Usa o SDK completo (tem compilador, restore, publish).
# É uma imagem "grande", mas ela não vai para produção.
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia só o .csproj primeiro e restaura as dependências.
# Por que separado? O Docker cacheia cada camada. Enquanto o
# .csproj não mudar, o 'restore' (lento) é reaproveitado do cache,
# mesmo que você altere o código. Otimização clássica de build.
COPY OrbitBoard.Api/OrbitBoard.Api.csproj OrbitBoard.Api/
RUN dotnet restore OrbitBoard.Api/OrbitBoard.Api.csproj

# Agora copia o restante do código-fonte.
COPY OrbitBoard.Api/ OrbitBoard.Api/

# Publica em modo Release, otimizado, na pasta /app/publish.
RUN dotnet publish OrbitBoard.Api/OrbitBoard.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ============================================================
# ESTÁGIO 2 — runtime: só o necessário para EXECUTAR
# Usa a imagem 'aspnet' (só o runtime, sem SDK/compilador).
# É aqui que mora a economia: ~110MB em vez de ~750MB.
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Traz APENAS o resultado publicado do estágio de build.
# Todo o SDK, código-fonte e ferramentas ficam para trás.
COPY --from=build /app/publish .

# Dentro do container, o launchSettings.json é IGNORADO.
# Precisamos dizer explicitamente em que porta o Kestrel escuta.
# Escolhemos 8080 (convenção para container; não exige root).
ENV ASPNETCORE_URLS=http://+:8080

# Documenta a porta que o container expõe (informativo).
EXPOSE 8080

# Comando que sobe a API quando o container inicia.
# O .dll tem o nome do assembly = nome do projeto.
ENTRYPOINT ["dotnet", "OrbitBoard.Api.dll"]