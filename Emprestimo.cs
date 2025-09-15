using System.Text.Json.Serialization;

namespace Ludoteca;

public class Emprestimo
{
    private const decimal MULTA_POR_DIA = 5.0m;
    private const int DIAS_PRAZO = 7;

    // [AV1-2] Construtor com validações
    public Emprestimo(Guid jogoId, Guid membroId, string nomeJogo, string nomeMembro)
    {
        if (jogoId == Guid.Empty)
            throw new ArgumentException("ID do jogo não pode ser vazio.", nameof(jogoId));
        
        if (membroId == Guid.Empty)
            throw new ArgumentException("ID do membro não pode ser vazio.", nameof(membroId));
        
        if (string.IsNullOrWhiteSpace(nomeJogo))
            throw new ArgumentException("Nome do jogo não pode estar vazio.", nameof(nomeJogo));
        
        if (string.IsNullOrWhiteSpace(nomeMembro))
            throw new ArgumentException("Nome do membro não pode estar vazio.", nameof(nomeMembro));

        Id = Guid.NewGuid();
        JogoId = jogoId;
        MembroId = membroId;
        NomeJogo = nomeJogo;
        NomeMembro = nomeMembro;
        DataEmprestimo = DateTime.Now;
        DataPrevistaDevolucao = DateTime.Now.AddDays(DIAS_PRAZO);
        DataDevolucao = null;
        Ativo = true;
        MultaCalculada = 0.0m;
    }

    // [AV1-2] Propriedades com encapsulamento
    public Guid Id { get; private set; }
    public Guid JogoId { get; private set; }
    public Guid MembroId { get; private set; }
    public string NomeJogo { get; private set; }
    public string NomeMembro { get; private set; }
    public DateTime DataEmprestimo { get; private set; }
    public DateTime DataPrevistaDevolucao { get; private set; }
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