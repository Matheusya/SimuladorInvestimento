# Simulador de Investimento - Fase 2

Bem-vindo à fase 2 do projeto! Esta versão inclui autenticação, histórico de simulações e exportação em PDF/Excel.

## 🚀 Novas Funcionalidades

### 1. **Autenticação de Usuários**
- Sistema de registro (Sign Up) com validação
- Login/Logout com Identity
- Senha criptografada no banco de dados
- Redirecionamento automático para login em páginas protegidas

### 2. **Histórico de Simulações**
- Cada simulação realizada é salva no banco de dados
- Associada automaticamente ao usuário autenticado
- Campo "Descrição" opcional para nomear simulações
- Página de histórico com todas as simulações do usuário
- Ordenadas por data (mais recentes primeiro)

### 3. **Exportação de Dados**
- **Excel (.xlsx)**: Exporta todas as simulações em uma planilha formatada
- **PDF**: Gera relatório em PDF com tabela de simulações
- Ambos os formatos incluem formatação profissional

## 📋 Como Usar

### Registrar uma Conta
1. Clique em **"Registrar"** no menu superior
2. Preencha email e senha
3. Clique em **"Registrar"**
4. Você será automaticamente autenticado

### Fazer uma Simulação
1. Preencha os campos: **Valor Inicial**, **Taxa de Juros**, **Tempo (meses)**
2. Opcionalmente, adicione uma **Descrição**
3. Clique em **"Calcular"**
4. A simulação será salva no histórico automaticamente
5. Veja um resumo das últimas simulações na home

### Acessar o Histórico
1. Clique em **"Histórico"** no menu
2. Veja todas as suas simulações com dados completos
3. Ordenadas por data mais recente

### Exportar Resultados
1. Clique em **"Exportar"** no menu
2. Escolha entre:
   - 📊 **Baixar Excel**: Para análise em planilha
   - 📄 **Baixar PDF**: Para compartilhar ou arquivar
3. O arquivo será baixado automático

## 🗄️ Estrutura do Banco de Dados

### Tabelas Principais

#### `AspNetUsers` (Identity)
- `Id`: Identificador único do usuário
- `UserName`: Email do usuário
- `Email`: Email único
- `PasswordHash`: Senha criptografada
- `DataCriacao`: Quando a conta foi criada

#### `SimulacoesHistorico`
- `Id`: Identificador único
- `UsuarioId`: Referência ao usuário (FK)
- `ValorInicial`: Valor inicial da simulação
- `TaxaJuros`: Taxa aplicada
- `TempoMeses`: Tempo em meses
- `MontanteFinal`: Resultado final
- `TotalJuros`: Juros acumulados
- `RentabilidadePercentual`: Rentabilidade %
- `Descricao`: Descrição da simulação (opcional)
- `DataSimulacao`: Quando foi feita

## 🔧 Arquitetura

### Páginas

- **`/Index`** (Home)
  - Requer autenticação
  - Formulário de simulação
  - Resumo das últimas simulações
  
- **`/Historico`** (Autenticado)
  - Lista completa de simulações
  - Tabela responsiva
  
- **`/Exportar`** (Autenticado)
  - Interface para escolher formato
  - Botões para Excel e PDF
  
- **`/Account/Register`**
  - Novo usuário
  - Validação de senha
  
- **`/Account/Login`**
  - Autenticação
  - Opção "Lembrar-me"
  
- **`/Account/Logout`**
  - Encerrar sessão

### Modelos

```csharp
// Usuario (estende IdentityUser)
public class Usuario : IdentityUser
{
    public DateTime DataCriacao { get; set; }
    public ICollection<SimulacaoHistorico> Simulacoes { get; set; }
}

// SimulacaoHistorico
public class SimulacaoHistorico
{
    public int Id { get; set; }
    public string UsuarioId { get; set; }
    public decimal ValorInicial { get; set; }
    public decimal TaxaJuros { get; set; }
    public int TempoMeses { get; set; }
    public decimal MontanteFinal { get; set; }
    public decimal TotalJuros { get; set; }
    public decimal RentabilidadePercentual { get; set; }
    public string Descricao { get; set; }
    public DateTime DataSimulacao { get; set; }
}
```

## 📦 Pacotes Instalados

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`: Autenticação
- `Microsoft.AspNetCore.Identity.UI`: UI de Identity
- `EPPlus`: Exportação Excel
- `QuestPDF`: Geração de PDF

## 🛡️ Segurança

- Senhas criptografadas com PBKDF2
- Histórico de simulações isolado por usuário
- Páginas protegidas com `[Authorize]`
- Validação de tokens CSRF automática
- HTTPS habilitado em produção

## 📝 Próximos Passos (Sugestões)

- [ ] Adicionar gráficos comparativos entre simulações
- [ ] Exportar com gráficos inclusos
- [ ] Compartilhamento de simulações
- [ ] Simulações em favoritos
- [ ] Análise por período
- [ ] Dashboard com estatísticas

## ✅ Testes

Para testar localmente:

```bash
# Restaurar pacotes
dotnet restore

# Aplicar migrations (já feito)
dotnet ef database update

# Rodar a aplicação
dotnet run

# Acessar
https://localhost:7000
```

Crie uma conta, faça uma simulação e teste as funcionalidades!

---

**Desenvolvido com ❤️ usando Razor Pages e .NET 10**
