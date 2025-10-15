namespace GestaoDeBarbearia.Domain.Entities;
public class ImpostoSimplesNacional
{
    public string Anexo { get; set; } = string.Empty;
    public string Faixa { get; set; } = string.Empty;
    public decimal FaturamentoLimiteInferior { get; set; }
    public decimal FaturamentoLimiteSuperior { get; set; }
    public decimal Aliquota { get; set; }
    public decimal ValorADeduzir { get; set; }
}
