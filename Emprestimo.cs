using System.Text.Json.Serialization;

namespace Ludoteca;

public class Emprestimo
{
    private const decimal MULTA_POR_DIA = 5.0m;
    private const int DIAS_PRAZO = 7;

    // Construtor vazio para deserialização JSON
    public Emprestimo()
    {
        Id = Guid.NewGuid();
        NomeJogo = string.Empty;
        NomeMembro = string.Empty;
        DataEmprestimo = DateTime.Now;
        DataPrevistaDevolucao = DateTime.Now.AddDays(7);
        Ativo = true;
    }

    // [AV1-2] Construtor com validações
    public Emprestimo(Guid jogoId, Guid membroId, int diasEmprestimo = 7)
    {
        if (jogoId == Guid.Empty)
            throw new ArgumentException("ID do jogo não pode ser vazio.", nameof(jogoId));
        
        if (membroId == Guid.Empty)
            throw new ArgumentException("ID do membro não pode ser vazio.", nameof(membroId));
        
        if (diasEmprestimo <= 0)
            throw new ArgumentException("Dias de empréstimo deve ser maior que zero.", nameof(diasEmprestimo));

        Id = Guid.NewGuid();
        JogoId = jogoId;
        MembroId = membroId;
        NomeJogo = string.Empty;
        NomeMembro = string.Empty;
        DataEmprestimo = DateTime.Now;
        DataPrevistaDevolucao = DateTime.Now.AddDays(diasEmprestimo);
        DataDevolucao = null;
        Ativo = true;
    }

    // [AV1-2] Propriedades com encapsulamento
    public Guid Id { get; set; }
    public Guid JogoId { get; set; }
    public Guid MembroId { get; set; }
    public string NomeJogo { get; set; } = string.Empty;
    public string NomeMembro { get; set; } = string.Empty;
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataPrevistaDevolucao { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public bool Ativo { get; set; }
    public decimal MultaCalculada { get; set; }

    public decimal CalcularMulta()
    {
        if (!Ativo || DataDevolucao.HasValue)
            return MultaCalculada;

        DateTime dataReferencia = DateTime.Now;
        if (dataReferencia <= DataPrevistaDevolucao)
            return 0.0m;

        int diasAtraso = (int)(dataReferencia - DataPrevistaDevolucao).TotalDays;
        MultaCalculada = diasAtraso * MULTA_POR_DIA;
        return MultaCalculada;
    }

    public void ProcessarDevolucao()
    {
        if (!Ativo)
            throw new InvalidOperationException("Empréstimo já foi devolvido.");

        DataDevolucao = DateTime.Now;
        Ativo = false;
        
        if (DataDevolucao.Value > DataPrevistaDevolucao)
        {
            int diasAtraso = (int)(DataDevolucao.Value - DataPrevistaDevolucao).TotalDays;
            MultaCalculada = diasAtraso * MULTA_POR_DIA;
        }
    }

    public bool EstaAtrasado()
    {
        return Ativo && DateTime.Now > DataPrevistaDevolucao;
    }

    public override string ToString()
    {
        string status = Ativo ? "Em andamento" : "Devolvido";
        string multa = MultaCalculada > 0 ? $" - Multa: R$ {MultaCalculada:F2}" : "";
        string prazo = EstaAtrasado() && Ativo ? " (ATRASADO)" : "";
        
        return $"{NomeJogo} → {NomeMembro} - {DataEmprestimo:dd/MM/yyyy} até {DataPrevistaDevolucao:dd/MM/yyyy} - {status}{multa}{prazo}";
    }
}