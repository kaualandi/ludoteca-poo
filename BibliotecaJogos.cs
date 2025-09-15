using System.Text.Json;

namespace Ludoteca;

public class BibliotecaJogos
{
    private readonly string caminhoArquivo = Path.Combine("data", "biblioteca.json");
    private readonly string caminhoRelatorio = "relatorio.txt";
    private readonly string caminhoLog = "debug.log";

    // [AV1-2] Propriedades com encapsulamento
    public List<Jogo> Jogos { get; private set; }
    public List<Membro> Membros { get; private set; }
    public List<Emprestimo> Emprestimos { get; private set; }

    // [AV1-2] Construtor
    public BibliotecaJogos()
    {
        Jogos = new List<Jogo>();
        Membros = new List<Membro>();
        Emprestimos = new List<Emprestimo>();
        
        // Criar diretório data se não existir
        string? diretorio = Path.GetDirectoryName(caminhoArquivo);
        if (!string.IsNullOrEmpty(diretorio))
            Directory.CreateDirectory(diretorio);
        
        // Carregar dados existentes
        Carregar();
    }

    // [AV1-3] Método de persistência com JSON
    public void Salvar()
    {
        try
        {
            DadosBiblioteca dados = new DadosBiblioteca
            {
                Jogos = this.Jogos,
                Membros = this.Membros,
                Emprestimos = this.Emprestimos,
                DataUltimaAtualizacao = DateTime.Now
            };

            // [AV1-3] Serialização JSON
            JsonSerializerOptions opcoes = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(dados, opcoes);
            File.WriteAllText(caminhoArquivo, json);
            
            EscreverLog($"Dados salvos com sucesso em {caminhoArquivo}");
        }
        catch (Exception ex)
        {
            EscreverLog($"Erro ao salvar dados: {ex.Message}");
            throw;
        }
    }

    // [AV1-3] Método de carregamento com JSON
    public void Carregar()
    {
        try
        {
            if (!File.Exists(caminhoArquivo))
            {
                EscreverLog("Arquivo de dados não encontrado. Iniciando com dados vazios.");
                return;
            }

            string json = File.ReadAllText(caminhoArquivo);
            
            // [AV1-3] Deserialização JSON
            DadosBiblioteca? dados = JsonSerializer.Deserialize<DadosBiblioteca>(json);
            
            if (dados != null)
            {
                Jogos = dados.Jogos ?? new List<Jogo>();
                Membros = dados.Membros ?? new List<Membro>();
                Emprestimos = dados.Emprestimos ?? new List<Emprestimo>();
                
                EscreverLog($"Dados carregados com sucesso: {Jogos.Count} jogos, {Membros.Count} membros, {Emprestimos.Count} empréstimos");
            }
        }
        catch (Exception ex)
        {
            EscreverLog($"Erro ao carregar dados: {ex.Message}");
            // Não propagar a exceção para permitir inicialização com dados vazios
        }
    }

    public void AdicionarJogo(Jogo jogo)
    {
        if (jogo == null)
            throw new ArgumentNullException(nameof(jogo));
        
        // Verificar se já existe um jogo com o mesmo nome
        if (Jogos.Any(j => j.Nome.Equals(jogo.Nome, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Já existe um jogo cadastrado com o nome '{jogo.Nome}'.");

        Jogos.Add(jogo);
        EscreverLog($"Jogo cadastrado: {jogo.Nome}");
    }

    public void AdicionarMembro(Membro membro)
    {
        if (membro == null)
            throw new ArgumentNullException(nameof(membro));
        
        // Verificar se já existe um membro com a mesma matrícula
        if (Membros.Any(m => m.Matricula.Equals(membro.Matricula, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Já existe um membro cadastrado com a matrícula '{membro.Matricula}'.");

        Membros.Add(membro);
        EscreverLog($"Membro cadastrado: {membro.Nome} ({membro.Matricula})");
    }

    public Emprestimo EmprestarJogo(Guid jogoId, Guid membroId)
    {
        Jogo? jogo = Jogos.FirstOrDefault(j => j.Id == jogoId);
        if (jogo == null)
            throw new ArgumentException("Jogo não encontrado.", nameof(jogoId));

        if (!jogo.Disponivel)
            throw new InvalidOperationException($"O jogo '{jogo.Nome}' não está disponível para empréstimo.");

        Membro? membro = Membros.FirstOrDefault(m => m.Id == membroId);
        if (membro == null)
            throw new ArgumentException("Membro não encontrado.", nameof(membroId));

        if (!membro.Ativo)
            throw new InvalidOperationException($"O membro '{membro.Nome}' não está ativo.");

        if (membro.MultaPendente > 0)
            throw new InvalidOperationException($"O membro '{membro.Nome}' possui multa pendente de R$ {membro.MultaPendente:F2}.");

        // Criar empréstimo
        Emprestimo emprestimo = new Emprestimo(jogoId, membroId, jogo.Nome, membro.Nome);
        Emprestimos.Add(emprestimo);
        
        // Marcar jogo como indisponível
        jogo.Disponivel = false;
        
        EscreverLog($"Empréstimo realizado: {jogo.Nome} para {membro.Nome}");
        return emprestimo;
    }

    public void DevolverJogo(Guid emprestimoId)
    {
        Emprestimo? emprestimo = Emprestimos.FirstOrDefault(e => e.Id == emprestimoId);
        if (emprestimo == null)
            throw new ArgumentException("Empréstimo não encontrado.", nameof(emprestimoId));

        if (!emprestimo.Ativo)
            throw new InvalidOperationException("Este empréstimo já foi devolvido.");

        // Processar devolução
        emprestimo.ProcessarDevolucao();
        
        // Marcar jogo como disponível
        Jogo? jogo = Jogos.FirstOrDefault(j => j.Id == emprestimo.JogoId);
        if (jogo != null)
            jogo.Disponivel = true;

        // Aplicar multa se houver
        if (emprestimo.MultaCalculada > 0)
        {
            Membro? membro = Membros.FirstOrDefault(m => m.Id == emprestimo.MembroId);
            if (membro != null)
                membro.MultaPendente += emprestimo.MultaCalculada;
        }

        EscreverLog($"Devolução processada: {emprestimo.NomeJogo} de {emprestimo.NomeMembro} - Multa: R$ {emprestimo.MultaCalculada:F2}");
    }

    public void GerarRelatorio()
    {
        try
        {
            using StreamWriter writer = new StreamWriter(caminhoRelatorio);
            
            writer.WriteLine("=== RELATÓRIO DA LUDOTECA ===");
            writer.WriteLine($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            writer.WriteLine();
            
            writer.WriteLine($"TOTAL DE JOGOS: {Jogos.Count}");
            writer.WriteLine($"Jogos disponíveis: {Jogos.Count(j => j.Disponivel)}");
            writer.WriteLine($"Jogos emprestados: {Jogos.Count(j => !j.Disponivel)}");
            writer.WriteLine();
            
            writer.WriteLine($"TOTAL DE MEMBROS: {Membros.Count}");
            writer.WriteLine($"Membros ativos: {Membros.Count(m => m.Ativo)}");
            writer.WriteLine($"Membros com multa: {Membros.Count(m => m.MultaPendente > 0)}");
            writer.WriteLine();
            
            writer.WriteLine($"EMPRÉSTIMOS:");
            writer.WriteLine($"Ativos: {Emprestimos.Count(e => e.Ativo)}");
            writer.WriteLine($"Finalizados: {Emprestimos.Count(e => !e.Ativo)}");
            writer.WriteLine($"Atrasados: {Emprestimos.Count(e => e.EstaAtrasado())}");
            writer.WriteLine();
            
            decimal totalMultas = Membros.Sum(m => m.MultaPendente);
            writer.WriteLine($"TOTAL EM MULTAS PENDENTES: R$ {totalMultas:F2}");
            
            EscreverLog($"Relatório gerado em {caminhoRelatorio}");
        }
        catch (Exception ex)
        {
            EscreverLog($"Erro ao gerar relatório: {ex.Message}");
            throw;
        }
    }

    private void EscreverLog(string mensagem)
    {
        try
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensagem}";
            File.AppendAllText(caminhoLog, logEntry + Environment.NewLine);
        }
        catch
        {
            // Ignorar erros de log para não afetar funcionalidade principal
        }
    }

    // Classe interna para serialização JSON
    private class DadosBiblioteca
    {
        public List<Jogo> Jogos { get; set; } = new();
        public List<Membro> Membros { get; set; } = new();
        public List<Emprestimo> Emprestimos { get; set; } = new();
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}