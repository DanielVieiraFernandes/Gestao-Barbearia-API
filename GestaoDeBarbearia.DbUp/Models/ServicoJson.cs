using System.Text.Json.Serialization;

namespace GestaoDeBarbearia.DbUp.Models;
internal class ServicoJson
{
    [JsonPropertyName("Nome")]
    public string name { get; set; } = string.Empty;

    [JsonPropertyName("Descricao")]
    public string description { get; set; } = string.Empty;

    [JsonPropertyName("PrecoEmCentavos")]
    public long price { get; set; }
}
