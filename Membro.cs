using System.Text.Json.Serialization;

namespace Ludoteca;

public class Membro
{
    // Construtor vazio para deserialização JSON
    public Membro()
    {
        Id = Guid.NewGuid();
        Nome = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        Matricula = string.Empty;
        DataCadastro = DateTime.Now;
        Ativo = true;
        MultaPendente = 0.0m;
    }

    // [AV1-2] Construtor com validações
    public Membro(string nome, string email, string telefone, string matricula)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome não pode estar vazio.", nameof(nome));
        
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Email inválido.", nameof(email));
        
        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("Telefone não pode estar vazio.", nameof(telefone));
        
        if (string.IsNullOrWhiteSpace(matricula))
            throw new ArgumentException("Matrícula não pode estar vazia.", nameof(matricula));

        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Matricula = matricula;
        DataCadastro = DateTime.Now;
        Ativo = true;
        MultaPendente = 0.0m;
    }

    // [AV1-2] Propriedades com encapsulamento
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
    public decimal MultaPendente { get; set; }

    public void PagarMulta(decimal valor, string metodoPagamento)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor deve ser positivo.", nameof(valor));
        
        if (valor > MultaPendente)
            throw new ArgumentException("Valor não pode ser maior que a multa pendente.", nameof(valor));

        MultaPendente -= valor;
        Console.WriteLine($"Multa paga: R$ {valor:F2} via {metodoPagamento}. Restante: R$ {MultaPendente:F2}");
    }

    public override string ToString()
    {
        string status = Ativo ? "Ativo" : "Inativo";
        string multa = MultaPendente > 0 ? $" - Multa: R$ {MultaPendente:F2}" : "";
        return $"{Nome} ({Matricula}) - {Email} - {status}{multa}";
    }
}