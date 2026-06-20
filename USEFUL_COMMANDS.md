# 🛠️ Comandos Úteis - Simulador de Investimento v2.0

## 🚀 Executar a Aplicação

```bash
# Rodar a aplicação (desenvolvimento)
dotnet run

# Rodar com hot reload
dotnet watch run

# Compilar sem executar
dotnet build

# Limpar e compilar
dotnet clean
dotnet build
```

## 🗄️ Banco de Dados

```bash
# Criar uma nova migration
dotnet ef migrations add NomedaMigracao

# Listar migrations
dotnet ef migrations list

# Aplicar migrations ao banco
dotnet ef database update

# Atualizar para uma migration específica
dotnet ef database update NomeMigration

# Remover a última migration
dotnet ef migrations remove

# Dropar o banco completamente
dotnet ef database drop

# Ver SQL que será executado
dotnet ef migrations script
```

## 📦 Gerenciador de Pacotes

```bash
# Restaurar pacotes
dotnet restore

# Adicionar novo pacote
dotnet add package NomePacote

# Remover pacote
dotnet remove package NomePacote

# Listar pacotes
dotnet list package

# Atualizar pacote específico
dotnet add package NomePacote --version 10.0.0

# Limpar cache NuGet
dotnet nuget locals all --clear
```

## 🧪 Testes

```bash
# Rodar todos os testes
dotnet test

# Rodar testes de projeto específico
dotnet test ./ProjetoTeste/ProjetoTeste.csproj

# Rodar com verbosidade
dotnet test --verbosity detailed
```

## 📋 Verificação de Projeto

```bash
# Verificar problemas
dotnet doctor

# Listar arquivos do projeto
dotnet list file

# Verificar referências
dotnet list reference
```

## 🔍 Debugging

```bash
# Rodar com debugger
dotnet run

# Attaching debugger no Visual Studio
# Usa F5 para debug
```

## 📝 Úteis do Projeto

```bash
# Instalar dependências (já fez, mas se precisar)
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package EPPlus
dotnet add package QuestPDF

# EF Core CLI
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

## 🚀 Publicação

```bash
# Publicar para Release
dotnet publish -c Release

# Publicar em pasta específica
dotnet publish -c Release -o ./publish

# Publicar com trimming (remove código não usado)
dotnet publish -c Release --use-current-runtime --self-contained
```

## 📊 Análise e Performance

```bash
# Mostrar informações da aplicação
dotnet --info

# Listar SDKs instalados
dotnet --list-sdks

# Listar runtimes instalados
dotnet --list-runtimes

# Benchmark da compilação
time dotnet build
```

## 🔐 Secrets (Senhas/Tokens)

```bash
# Inicializar secrets
dotnet user-secrets init

# Adicionar secret
dotnet user-secrets set "Chave" "Valor"

# Listar secrets
dotnet user-secrets list

# Remover secret
dotnet user-secrets remove "Chave"

# Limpar todos
dotnet user-secrets clear
```

## 📱 Formato/Lint

```bash
# Verificar estilo de código
dotnet format --verify-no-changes

# Aplicar formatação
dotnet format

# Verificar apenas
dotnet format --no-restore --verify-no-changes
```

---

## 🎯 Fluxo de Desenvolvimento Recomendado

### Para Adicionar Nova Funcionalidade

1. **Criar migration** (se mexer no banco):
   ```bash
   dotnet ef migrations add NovaMigracao
   ```

2. **Aplicar migration**:
   ```bash
   dotnet ef database update
   ```

3. **Criar/modificar arquivo**:
   ```bash
   Editar arquivo
   ```

4. **Testar compilação**:
   ```bash
   dotnet build
   ```

5. **Executar aplicação**:
   ```bash
   dotnet run
   ```

6. **Testar no navegador**:
   ```
   https://localhost:7000
   ```

### Para Debugar Problema

1. **Verificar compilação**:
   ```bash
   dotnet build
   ```

2. **Restaurar pacotes**:
   ```bash
   dotnet restore
   ```

3. **Limpar cache**:
   ```bash
   dotnet clean
   ```

4. **Recompilar**:
   ```bash
   dotnet build
   ```

5. **Executar com watch**:
   ```bash
   dotnet watch run
   ```

---

## 📖 Recursos Úteis

### Documentação Oficial
- https://docs.microsoft.com/dotnet
- https://docs.microsoft.com/aspnet
- https://entityframeworkcore.io

### Pacotes
- **EPPlus**: https://www.epplussoftware.com
- **QuestPDF**: https://www.questpdf.com
- **Bootstrap**: https://getbootstrap.com
- **Chart.js**: https://www.chartjs.org

### Comunidade
- Stack Overflow: [tag: asp.net-core]
- GitHub Discussions
- Microsoft Learn

---

## ⚠️ Troubleshooting Comuns

### Erro: "dotnet: command not found"
```bash
# Instalar .NET SDK
# macOS: brew install dotnet
# Windows: https://dotnet.microsoft.com/download
# Linux: https://docs.microsoft.com/dotnet/core/install/linux
```

### Erro: "The database already contains a migration named..."
```bash
# Remover migration
dotnet ef migrations remove

# Recriar
dotnet ef migrations add NovaDescricao
```

### Erro: "Could not connect to database"
```bash
# Verificar connection string em appsettings.json
# Verificar se SQL Server está rodando
# Resetar banco: dotnet ef database drop
# Recriar: dotnet ef database update
```

### Erro: "Port is already in use"
```bash
# Mudar porta em launchSettings.json
# Ou encerrar processo na porta:
# Linux/Mac: lsof -i :7000
# Windows: netstat -ano | findstr :7000
```

---

## 🎮 Atalhos Úteis (Visual Studio)

```
Ctrl+Shift+B    → Compilar
F5              → Debug
Ctrl+F5         → Executar sem debug
F10             → Step Over
F11             → Step Into
Ctrl+Alt+Z      → Attach to Process
Ctrl+K+C        → Comentar
Ctrl+K+U        → Descomentar
```

---

## 📊 Exemplo: Adicionar Novo Campo ao Histórico

```bash
# 1. Modificar modelo
# Editar: SimulacaoHistorico.cs
# Adicionar propriedade

# 2. Criar migration
dotnet ef migrations add AdicionarCampoNoHistorico

# 3. Revisar o arquivo gerado
# Arquivo em: Migrations/[timestamp]_AdicionarCampoNoHistorico.cs

# 4. Aplicar ao banco
dotnet ef database update

# 5. Compilar
dotnet build

# 6. Testar
dotnet run
```

---

## 🔄 Sincronizar com Git

```bash
# Status
git status

# Adicionar tudo
git add .

# Commit
git commit -m "Mensagem descritiva"

# Push
git push origin master

# Pull (atualizar)
git pull origin master
```

---

## 💾 Backup do Banco de Dados

```bash
# SQL Server (via SSMS)
# Clique direito em banco → Tasks → Backup

# Via linha de comando (sqlcmd)
sqlcmd -S (localdb)\mssqllocaldb -Q "BACKUP DATABASE SimuladorInvestimentoDB TO DISK='C:\backup.bak'"
```

---

**Dica**: Adicione este arquivo aos favoritos para referência rápida! 📌

---

*Última atualização: 5 de junho de 2026*
*Versão: 2.0*
