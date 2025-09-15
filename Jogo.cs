using System.Text.Json.Serialization;

namespace Ludoteca;

public class Jogo
{
    // [AV1-2] Construtor com validações
    public Jogo(string nome, int anoPublicacao, string categoria, int jogadoresMin = 1, int jogadoresMax = 10)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do jogo não pode estar vazio.", nameof(nome));
        
        if (anoPublicacao < 1900 || anoPublicacao > DateTime.Now.Year)
            throw new ArgumentException("Ano de publicação inválido.", nameof(anoPublicacao));
        
        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("Categoria não pode estar vazia.", nameof(categoria));
        
        if (jogadoresMin < 1)
            throw new ArgumentException("Número mínimo de jogadores deve ser pelo menos 1.", nameof(jogadoresMin));
        
        if (jogadoresMax < jogadoresMin)
            throw new ArgumentException("Número máximo de jogadores deve ser maior ou igual ao mínimo.", nameof(jogadoresMax));

        Id = Guid.NewGuid();
        Nome = nome;
        AnoPublicacao = anoPublicacao;
        Categoria = categoria;
        JogadoresMin = jogadoresMin;
        JogadoresMax = jogadoresMax;
        DataCadastro = DateTime.Now;
        Disponivel = true;
    }

    // [AV1-2] Propriedades com encapsulamento
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public int AnoPublicacao { get; private set; }
    public string Categoria { get; private set; }
    public int JogadoresMin { get; private set; }
    public int JogadoresMax { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Disponivel { get; set; }

    public override string ToString()
    {
        string status = Disponivel ? "Disponível" : "Emprestado";
        return $"{Nome} ({AnoPublicacao}) - {Categoria} - {JogadoresMin}-{JogadoresMax} jogadores - {status}";
    }
}