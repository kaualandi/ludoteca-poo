# Ludoteca .NET

Sistema de controle de empréstimo de jogos de tabuleiro desenvolvido em C# .NET 9 para um clube universitário.

## 📋 Funcionalidades

- ✅ Cadastro de jogos de tabuleiro
- ✅ Cadastro de membros do clube
- ✅ Sistema de empréstimo e devolução
- ✅ Cálculo automático de multas por atraso
- ✅ Pagamento de multas (PIX ou dinheiro)
- ✅ Persistência de dados em JSON
- ✅ Geração de relatórios
- ✅ Sistema de logs para debug
- ✅ Validações e tratamento de exceções

## 🏗️ Arquitetura

### Diagrama UML
O diagrama UML das classes está disponível em: `evidencias/av1/diagrama-uml.png`

### Classes Implementadas

#### 1. Classe `Jogo` (Jogo.cs)
- **Construtor**: Linhas 8-29 com validações completas
- **Propriedades validadas**: 
  - Nome (não pode estar vazio)
  - AnoPublicacao (entre 1900 e ano atual)
  - Categoria (não pode estar vazia)
  - JogadoresMin/Max (valores lógicos)

#### 2. Classe `Membro` (Membro.cs)
- **Construtor**: Linhas 8-29 com validações
- **Propriedades validadas**:
  - Nome (não pode estar vazio)
  - Email (deve conter @)
  - Telefone (não pode estar vazio)
  - Matrícula (não pode estar vazia)

#### 3. Classe `Emprestimo` (Emprestimo.cs)
- **Construtor**: Linhas 11-32 com validações
- **Propriedades validadas**:
  - IDs de jogo e membro (não podem ser vazios)
  - Nomes (não podem estar vazios)
- **Métodos especiais**:
  - `CalcularMulta()`: Calcula multa por atraso (R$ 5,00/dia)
  - `ProcessarDevolucao()`: Processa a devolução
  - `EstaAtrasado()`: Verifica se está em atraso

#### 4. Classe `BibliotecaJogos` (BibliotecaJogos.cs)
- **Construtor**: Linhas 17-27
- **Propriedades validadas**: Listas encapsuladas com `private set`
- **Métodos principais**:
  - `Salvar()` e `Carregar()`: Persistência em JSON (linhas 30-94)
  - `EmprestarJogo()`: Validações completas de empréstimo
  - `DevolverJogo()`: Processamento de devoluções
  - `GerarRelatorio()`: Geração de relatório completo

## 💾 Persistência de Dados

O sistema utiliza `System.Text.Json` para persistência:
- **Arquivo**: `data/biblioteca.json`
- **Serialização**: Comentários `// [AV1-3]` nas linhas 38-45 e 73-74
- **Carregamento automático**: Na inicialização do sistema
- **Salvamento automático**: Ao sair do programa

## 🎮 Menu do Sistema

Menu interativo com as seguintes opções (comentários `// [AV1-4]`):
- **Linha 18**: Menu principal
- **Linha 19**: [AV1-4-Cadastrar-Jogo] - Cadastrar jogo
- **Linha 20**: [AV1-4-Cadastrar-Membro] - Cadastrar membro  
- **Linha 21**: [AV1-4-Listar] - Listar jogos
- **Linha 22**: [AV1-4-Emprestar] - Emprestar jogo
- **Linha 23**: [AV1-4-Devolver] - Devolver jogo
- **Linha 24**: [AV1-4-Relatorio] - Gerar relatório
- **Linha 27**: [AV1-4-Sair] - Sair do sistema

## 🛡️ Tratamento de Exceções

Implementação completa com comentários `// [AV1-5]`:
- **ArgumentException**: Para validações de entrada inválida
- **InvalidOperationException**: Para operações não permitidas
- **FormatException**: Para erros de formato de dados
- **Blocos try/catch**: Em todas as operações críticas

### Exemplos de Validações:
- ❌ Tentar emprestar jogo já emprestado
- ❌ Membro com multa pendente não pode emprestar
- ❌ Dados de entrada inválidos ou vazios
- ❌ Operações em registros inexistentes

## 📊 Relatórios e Logs

### Relatório (`relatorio.txt`)
- Estatísticas completas do sistema
- Contadores de jogos, membros e empréstimos
- Total de multas pendentes
- Situação atual da ludoteca

### Log de Debug (`debug.log`)
- Registros timestampados de todas as operações
- Erros e exceções capturadas
- Histórico de ações realizadas

## 🚀 Como Executar

```bash
# Compilar o projeto
dotnet build

# Executar o sistema
dotnet run
```

## 📁 Estrutura do Projeto

```
Ludoteca/
├── Program.cs              # Menu principal e interface
├── Jogo.cs                # Classe Jogo
├── Membro.cs              # Classe Membro
├── Emprestimo.cs          # Classe Emprestimo
├── BibliotecaJogos.cs     # Controlador principal
├── data/                  # Dados persistidos
│   └── biblioteca.json    # Arquivo de dados
├── evidencias/av1/        # Screenshots da avaliação
├── relatorio.txt          # Relatório do sistema
├── debug.log              # Log de debug
└── README.md              # Este arquivo
```

## 🎬 Demonstração

**Vídeo de demonstração**: [Link será adicionado aqui]

### Evidências (Screenshots)
As evidências solicitadas estão disponíveis em: `evidencias/av1/`
1. `menu-principal.png` - Menu principal funcionando
2. `cadastro-jogo.png` - Cadastro de jogo
3. `emprestimo-sucesso.png` - Empréstimo realizado
4. `relatorio-gerado.png` - Relatório gerado

## ⚙️ Especificações Técnicas

- **Framework**: .NET 9.0
- **Linguagem**: C# com nullable enabled
- **Persistência**: System.Text.Json
- **Arquitetura**: Console Application
- **Padrões**: Encapsulamento, validações, tratamento de exceções

## 👥 Desenvolvedores

[Adicionar nomes dos membros do grupo aqui]

---

*Desenvolvido para a disciplina de Programação Orientada a Objetos*