# 🚀 Guia Rápido de Uso - Simulador de Investimento v2.0

## 📋 O que foi entregue?

Sua aplicação Razor Pages agora tem:

### ✅ 1. Histórico de Simulações
- Banco de dados salva automaticamente cada simulação
- Tabela `SimulacoesHistorico` com todos os dados
- Cada simulação vinculada ao usuário

### ✅ 2. Autenticação de Usuários
- Sistema de login/registro completo
- Senhas seguras criptografadas
- Histórico individual por usuário
- Links de autenticação no menu

### ✅ 3. Exportação em Formatos
- **PDF**: Relatório profissional com tabela
- **Excel**: Planilha formatada pronta para análise

---

## 🎮 Como Usar (Passo a Passo)

### Primeiro Acesso

1. Execute a aplicação: `dotnet run`
2. Abra `https://localhost:7000` no navegador
3. Veja o aviso pedindo para se registrar

### Criar uma Conta

1. Clique em **"Registrar"** no canto superior direito
2. Preencha:
   - Email: `seu-email@exemplo.com`
   - Senha: `mínimo 6 caracteres`
   - Confirmar Senha
3. Clique em **"Registrar"**
4. Você estará automaticamente logado ✅

### Fazer uma Simulação

1. Preencha o formulário na home:
   - **Valor Inicial**: ex: 1000.00
   - **Taxa de Juros Mensal (%)**: ex: 1.5
   - **Tempo (meses)**: ex: 12
   - **Descrição** (opcional): ex: "Teste com juros 1.5% ao mês"

2. Clique em **"Calcular"**
3. Veja os resultados (Montante Final, Juros, Rentabilidade %)
4. Veja o gráfico da evolução
5. **Automaticamente salvo no histórico** 📊

### Consultar o Histórico

1. Clique em **"Histórico"** no menu superior
2. Veja uma tabela com:
   - Data da simulação
   - Todos os parâmetros (Valor Inicial, Taxa, Tempo)
   - Resultados (Montante, Juros, Rentabilidade)
3. Simulações ordenadas por data (mais recentes primeiro)

### Exportar Seus Dados

#### Opção 1: Excel
1. Clique em **"Exportar"** no menu
2. Clique em **"📊 Baixar Excel"**
3. Arquivo `Simulacoes_YYYYMMDD_HHMMSS.xlsx` será baixado
4. Abra no Excel/Google Sheets para análise

#### Opção 2: PDF
1. Clique em **"Exportar"** no menu
2. Clique em **"📄 Baixar PDF"**
3. Arquivo `Simulacoes_YYYYMMDD_HHMMSS.pdf` será baixado
4. Abra no navegador para visualizar/imprimir

### Fazer Logout

1. Clique em **"Sair"** no canto superior direito
2. Será redirecionado para a home
3. Agora o botão "Calcular" estará desabilitado

---

## 🗄️ Dados Salvos no Banco

Cada simulação registra:
- ✅ Data e hora
- ✅ Valor inicial investido
- ✅ Taxa de juros aplicada
- ✅ Período em meses
- ✅ Montante final
- ✅ Total de juros gerados
- ✅ Rentabilidade em %
- ✅ Descrição personalizada
- ✅ ID do usuário (para histórico individual)

---

## 🔒 Segurança

- ✅ Senhas criptografadas
- ✅ Sessões seguras
- ✅ Histórico isolado por usuário
- ✅ CSRF token automático
- ✅ Validação de entrada

---

## 📱 Estrutura de URLs

| Página | URL | Requer Login |
|--------|-----|--------------|
| Home | `/` | Sim |
| Histórico | `/Historico` | Sim |
| Exportar | `/Exportar` | Sim |
| Registrar | `/Account/Register` | Não |
| Login | `/Account/Login` | Não |
| Logout | `/Account/Logout` | Sim |

---

## 🐛 Troubleshooting

### "Página não encontrada" ao clicar em Histórico
- ✅ Verifique se você está logado
- ✅ A url deve ser: `https://localhost:7000/Historico`

### "Botão Calcular desabilitado"
- ✅ Você precisa fazer login primeiro
- ✅ Clique em "Registrar" para criar uma conta

### "Excel não abre"
- ✅ Use Excel 2016 ou posterior
- ✅ Ou use Google Sheets online

### "Histórico vazio"
- ✅ Você não tem simulações registradas ainda
- ✅ Faça uma simulação na home para aparecer

---

## 💡 Dicas

1. Use a **Descrição** para organizar simulações por propósito
2. Compare simulações diferentes usando **Excel**
3. Compartilhe relatórios em **PDF** com outras pessoas
4. O **histórico aparece na home** para acesso rápido (últimas 5)
5. Todos os dados são seus - só você vê seu histórico

---

## 🎯 Próximas Ideias de Uso

- Simule diferentes taxas para encontrar a melhor
- Compare investimentos diferentes exportando para Excel
- Imprima relatórios em PDF para arquivar
- Compartilhe simulações com colegas via PDF
- Teste cenários de "e se..." com diferentes parâmetros

---

## 📞 Suporte

Se algo não funcionar:
1. Verifique se está logado (ícone de usuário no canto superior)
2. Teste no navegador Chrome ou Edge
3. Limpe o cache do navegador (Ctrl+Shift+Delete)
4. Reinicie a aplicação: `dotnet run`

---

**Pronto para começar? Acesse https://localhost:7000 e crie sua conta! 🚀**

Version: 2.0 (Fase 2 Completa)
Data: 2026-06-05
