using Ludoteca;

// Inicializar biblioteca de jogos
BibliotecaJogos biblioteca = new BibliotecaJogos();

Console.WriteLine("=== LUDOTECA .NET ===");
Console.WriteLine("Sistema inicializado com sucesso!");
Console.WriteLine();

// Loop principal do menu
bool continuar = true;
while (continuar)
{
    try
    {
        // [AV1-4] Menu principal
        Console.WriteLine("=== LUDOTECA .NET ===");
        Console.WriteLine("1 Cadastrar jogo");       // [AV1-4-Cadastrar-Jogo]
        Console.WriteLine("2 Cadastrar membro");     // [AV1-4-Cadastrar-Membro]
        Console.WriteLine("3 Listar jogos");         // [AV1-4-Listar]
        Console.WriteLine("4 Emprestar jogo");       // [AV1-4-Emprestar]
        Console.WriteLine("5 Devolver jogo");        // [AV1-4-Devolver]
        Console.WriteLine("6 Gerar relatório");      // [AV1-4-Relatorio]
        Console.WriteLine("7 Listar membros");       // [AV1-4-Listar-Membros]
        Console.WriteLine("8 Pagar multa");          // [AV1-4-Pagar-Multa]
        Console.WriteLine("9 Salvar dados");         // [AV1-4-Salvar]
        Console.WriteLine("0 Sair");                 // [AV1-4-Sair]
        Console.Write("Opção: ");

        string? opcao = Console.ReadLine();
        Console.WriteLine();

        switch (opcao)
        {
            case "1": // [AV1-4-Cadastrar-Jogo]
                CadastrarJogo(biblioteca);
                break;
            case "2": // [AV1-4-Cadastrar-Membro]
                CadastrarMembro(biblioteca);
                break;
            case "3": // [AV1-4-Listar]
                ListarJogos(biblioteca);
                break;
            case "4": // [AV1-4-Emprestar]
                EmprestarJogo(biblioteca);
                break;
            case "5": // [AV1-4-Devolver]
                DevolverJogo(biblioteca);
                break;
            case "6": // [AV1-4-Relatorio]
                GerarRelatorio(biblioteca);
                break;
            case "7": // [AV1-4-Listar-Membros]
                ListarMembros(biblioteca);
                break;
            case "8": // [AV1-4-Pagar-Multa]
                PagarMulta(biblioteca);
                break;
            case "9": // [AV1-4-Salvar]
                SalvarDados(biblioteca);
                break;
            case "0": // [AV1-4-Sair]
                continuar = false;
                Console.WriteLine("Salvando dados automaticamente...");
                biblioteca.Salvar();
                Console.WriteLine("Obrigado por usar a Ludoteca .NET!");
                break;
            default:
                Console.WriteLine("❌ Opção inválida! Por favor, escolha uma opção entre 0 e 9.");
                break;
        }

        if (continuar)
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            try 
            {
                Console.ReadKey();
            }
            catch
            {
                Console.Read(); // Fallback para entrada redirecionada
            }
            Console.Clear();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        try 
        {
            Console.ReadKey();
        }
        catch
        {
            Console.Read(); // Fallback para entrada redirecionada
        }
        Console.Clear();
    }
}

// Métodos auxiliares do menu
static void CadastrarJogo(BibliotecaJogos biblioteca)
{
    try
    {
        Console.WriteLine("=== CADASTRAR JOGO ===");
        
        Console.Write("Nome do jogo: ");
        string? nome = Console.ReadLine();
        
        Console.Write("Ano de publicação: ");
        string? anoStr = Console.ReadLine();
        
        Console.Write("Categoria: ");
        string? categoria = Console.ReadLine();
        
        Console.Write("Número mínimo de jogadores (padrão: 1): ");
        string? minStr = Console.ReadLine();
        
        Console.Write("Número máximo de jogadores (padrão: 10): ");
        string? maxStr = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome) || 
            string.IsNullOrWhiteSpace(anoStr) || 
            string.IsNullOrWhiteSpace(categoria))
        {
            throw new ArgumentException("Todos os campos obrigatórios devem ser preenchidos.");
        }

        int ano = int.Parse(anoStr);
        int min = string.IsNullOrWhiteSpace(minStr) ? 1 : int.Parse(minStr);
        int max = string.IsNullOrWhiteSpace(maxStr) ? 10 : int.Parse(maxStr);

        Jogo novoJogo = new Jogo(nome, ano, categoria, min, max);
        biblioteca.AdicionarJogo(novoJogo);
        
        Console.WriteLine($"✅ Jogo '{nome}' cadastrado com sucesso!");
    }
    // [AV1-5] Tratamento de exceções
    catch (ArgumentException ex)
    {
        Console.WriteLine($"❌ Dados inválidos: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"❌ Operação inválida: {ex.Message}");
    }
    catch (FormatException)
    {
        Console.WriteLine("❌ Formato de número inválido. Verifique os valores inseridos.");
    }
}

static void CadastrarMembro(BibliotecaJogos biblioteca)
{
    try
    {
        Console.WriteLine("=== CADASTRAR MEMBRO ===");
        
        Console.Write("Nome: ");
        string? nome = Console.ReadLine();
        
        Console.Write("Email: ");
        string? email = Console.ReadLine();
        
        Console.Write("Telefone: ");
        string? telefone = Console.ReadLine();
        
        Console.Write("Matrícula: ");
        string? matricula = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome) || 
            string.IsNullOrWhiteSpace(email) || 
            string.IsNullOrWhiteSpace(telefone) || 
            string.IsNullOrWhiteSpace(matricula))
        {
            throw new ArgumentException("Todos os campos são obrigatórios.");
        }

        Membro novoMembro = new Membro(nome, email, telefone, matricula);
        biblioteca.AdicionarMembro(novoMembro);
        
        Console.WriteLine($"✅ Membro '{nome}' cadastrado com sucesso!");
    }
    // [AV1-5] Tratamento de exceções
    catch (ArgumentException ex)
    {
        Console.WriteLine($"❌ Dados inválidos: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"❌ Operação inválida: {ex.Message}");
    }
}

static void ListarJogos(BibliotecaJogos biblioteca)
{
    Console.WriteLine("=== LISTA DE JOGOS ===");
    
    if (!biblioteca.Jogos.Any())
    {
        Console.WriteLine("Nenhum jogo cadastrado.");
        return;
    }

    Console.WriteLine($"Total de jogos: {biblioteca.Jogos.Count}");
    Console.WriteLine();
    
    foreach (Jogo jogo in biblioteca.Jogos.OrderBy(j => j.Nome))
    {
        Console.WriteLine($"• {jogo}");
    }
}

static void ListarMembros(BibliotecaJogos biblioteca)
{
    Console.WriteLine("=== LISTA DE MEMBROS ===");
    
    if (!biblioteca.Membros.Any())
    {
        Console.WriteLine("Nenhum membro cadastrado.");
        return;
    }

    Console.WriteLine($"Total de membros: {biblioteca.Membros.Count}");
    Console.WriteLine();
    
    foreach (Membro membro in biblioteca.Membros.OrderBy(m => m.Nome))
    {
        Console.WriteLine($"• {membro}");
    }
}

static void EmprestarJogo(BibliotecaJogos biblioteca)
{
    try
    {
        Console.WriteLine("=== EMPRESTAR JOGO ===");
        
        // Listar jogos disponíveis
        List<Jogo> jogosDisponiveis = biblioteca.Jogos.Where(j => j.Disponivel).ToList();
        if (!jogosDisponiveis.Any())
        {
            Console.WriteLine("Não há jogos disponíveis para empréstimo.");
            return;
        }

        Console.WriteLine("Jogos disponíveis:");
        for (int i = 0; i < jogosDisponiveis.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {jogosDisponiveis[i]}");
        }
        
        Console.Write("Escolha o número do jogo: ");
        string? jogoEscolhido = Console.ReadLine();
        
        if (!int.TryParse(jogoEscolhido, out int indiceJogo) || 
            indiceJogo < 1 || indiceJogo > jogosDisponiveis.Count)
        {
            throw new ArgumentException("Número do jogo inválido.");
        }

        Jogo jogo = jogosDisponiveis[indiceJogo - 1];

        // Listar membros ativos
        List<Membro> membrosAtivos = biblioteca.Membros.Where(m => m.Ativo).ToList();
        if (!membrosAtivos.Any())
        {
            Console.WriteLine("Não há membros ativos para emprestar jogos.");
            return;
        }

        Console.WriteLine("\nMembros ativos:");
        for (int i = 0; i < membrosAtivos.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {membrosAtivos[i]}");
        }
        
        Console.Write("Escolha o número do membro: ");
        string? membroEscolhido = Console.ReadLine();
        
        if (!int.TryParse(membroEscolhido, out int indiceMembro) || 
            indiceMembro < 1 || indiceMembro > membrosAtivos.Count)
        {
            throw new ArgumentException("Número do membro inválido.");
        }

        Membro membro = membrosAtivos[indiceMembro - 1];

        Emprestimo emprestimo = biblioteca.EmprestarJogo(jogo.Id, membro.Id);
        Console.WriteLine($"✅ Empréstimo realizado com sucesso!");
        Console.WriteLine($"Prazo de devolução: {emprestimo.DataPrevistaDevolucao:dd/MM/yyyy}");
    }
    // [AV1-5] Tratamento de exceções
    catch (ArgumentException ex)
    {
        Console.WriteLine($"❌ Dados inválidos: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"❌ Operação inválida: {ex.Message}");
    }
}

static void DevolverJogo(BibliotecaJogos biblioteca)
{
    try
    {
        Console.WriteLine("=== DEVOLVER JOGO ===");
        
        List<Emprestimo> emprestimosAtivos = biblioteca.Emprestimos.Where(e => e.Ativo).ToList();
        if (!emprestimosAtivos.Any())
        {
            Console.WriteLine("Não há empréstimos ativos para devolver.");
            return;
        }

        Console.WriteLine("Empréstimos ativos:");
        for (int i = 0; i < emprestimosAtivos.Count; i++)
        {
            Emprestimo emp = emprestimosAtivos[i];
            string multaInfo = emp.CalcularMulta() > 0 ? $" - MULTA: R$ {emp.CalcularMulta():F2}" : "";
            Console.WriteLine($"{i + 1}. {emp}{multaInfo}");
        }
        
        Console.Write("Escolha o número do empréstimo: ");
        string? emprestimoEscolhido = Console.ReadLine();
        
        if (!int.TryParse(emprestimoEscolhido, out int indiceEmprestimo) || 
            indiceEmprestimo < 1 || indiceEmprestimo > emprestimosAtivos.Count)
        {
            throw new ArgumentException("Número do empréstimo inválido.");
        }

        Emprestimo emprestimo = emprestimosAtivos[indiceEmprestimo - 1];
        decimal multa = emprestimo.CalcularMulta();
        
        biblioteca.DevolverJogo(emprestimo.Id);
        
        Console.WriteLine($"✅ Devolução processada com sucesso!");
        if (multa > 0)
        {
            Console.WriteLine($"💰 Multa aplicada: R$ {multa:F2}");
            Console.WriteLine("O membro deve pagar a multa antes de realizar novos empréstimos.");
        }
    }
    // [AV1-5] Tratamento de exceções
    catch (ArgumentException ex)
    {
        Console.WriteLine($"❌ Dados inválidos: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"❌ Operação inválida: {ex.Message}");
    }
}

static void PagarMulta(BibliotecaJogos biblioteca)
{
    try
    {
        Console.WriteLine("=== PAGAR MULTA ===");
        
        List<Membro> membrosComMulta = biblioteca.Membros.Where(m => m.MultaPendente > 0).ToList();
        if (!membrosComMulta.Any())
        {
            Console.WriteLine("Não há membros com multas pendentes.");
            return;
        }

        Console.WriteLine("Membros com multas:");
        for (int i = 0; i < membrosComMulta.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {membrosComMulta[i]}");
        }
        
        Console.Write("Escolha o número do membro: ");
        string? membroEscolhido = Console.ReadLine();
        
        if (!int.TryParse(membroEscolhido, out int indiceMembro) || 
            indiceMembro < 1 || indiceMembro > membrosComMulta.Count)
        {
            throw new ArgumentException("Número do membro inválido.");
        }

        Membro membro = membrosComMulta[indiceMembro - 1];
        
        Console.WriteLine($"Multa pendente: R$ {membro.MultaPendente:F2}");
        Console.Write("Valor a pagar: R$ ");
        string? valorStr = Console.ReadLine();
        
        if (!decimal.TryParse(valorStr, out decimal valor))
        {
            throw new ArgumentException("Valor inválido.");
        }

        Console.WriteLine("Método de pagamento:");
        Console.WriteLine("1. PIX");
        Console.WriteLine("2. Dinheiro");
        Console.Write("Escolha: ");
        string? metodoEscolha = Console.ReadLine();
        
        string metodo = metodoEscolha switch
        {
            "1" => "PIX",
            "2" => "Dinheiro",
            _ => throw new ArgumentException("Método de pagamento inválido.")
        };

        membro.PagarMulta(valor, metodo);
        Console.WriteLine("✅ Pagamento processado com sucesso!");
    }
    // [AV1-5] Tratamento de exceções
    catch (ArgumentException ex)
    {
        Console.WriteLine($"❌ Dados inválidos: {ex.Message}");
    }
    catch (FormatException)
    {
        Console.WriteLine("❌ Formato de valor inválido.");
    }
}

static void GerarRelatorio(BibliotecaJogos biblioteca)
{
    try
    {
        biblioteca.GerarRelatorio();
        Console.WriteLine("✅ Relatório gerado com sucesso em 'relatorio.txt'!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao gerar relatório: {ex.Message}");
    }
}

static void SalvarDados(BibliotecaJogos biblioteca)
{
    try
    {
        biblioteca.Salvar();
        Console.WriteLine("✅ Dados salvos com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao salvar dados: {ex.Message}");
    }
}
