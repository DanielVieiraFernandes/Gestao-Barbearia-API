using System.Text.Json.Serialization;

namespace GestaoDeBarbearia.DbUp.Models;
internal class SimplesNacional
{
    public List<Tabela> Tabelas
    { get; set; } = [];
}

class Tabela
{
    public string Anexo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public List<FaixaAliquota> Faixas { get; set; } = [];
}

class FaixaAliquota
{
    public string Faixa { get; set; } = string.Empty;

    [JsonPropertyName("faturamento_ate")]
    public decimal FaturamentoAte { get; set; }
    public decimal Aliquota { get; set; }

    [JsonPropertyName("parcela_a_deduzir")]
    public decimal ParcelaADeduzir { get; set; }
}



