using GestaoDeBarbearia.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Communication.Requests;
public class RequestEmployeeJson
{
    [Required(ErrorMessage = "É obrigatório informar o nome do funcionário para identificação")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "O nome do funcionário deve ter entre 3 e 255 caracteres")]
    public string Name { get; set; } = string.Empty;

    // Nenhum DDD iniciado por 0 é aceito, e nenhum número de telefone pode iniciar com 0 ou 1.
    // Exemplos válidos: +55 (11) 98888-8888 / 9999-9999 / 21 98888-8888 / 5511988888888
    // 5519989993437

    [RegularExpression
        (
        "^(?:(?:\\+|00)?(55)\\s?)?(?:\\(?([1-9][0-9])\\)?\\s?)?(?:((?:9\\d|[2-9])\\d{3})\\-?(\\d{4}))$"
        , ErrorMessage = "Telefone inválido")]
    [StringLength(15, MinimumLength = 9, ErrorMessage = "Quantidade de caracteres inválidos para um telefone, deve ter entre 9 e 15 caracteres")]
    public string Telephone { get; set; } = string.Empty;

    [Required(ErrorMessage = "É obrigatório informar o E-mail do funcionário")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "É obrigatório informar a senha do funcionário")]
    [StringLength(8, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 8 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "É obrigatório informar o salário do funcionário")]
    [Range(0.1, (double)decimal.MaxValue, ErrorMessage = "Valor inválido para salário")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "É obrigatório informar o cargo do funcionário")]
    [EnumDataType(typeof(EmployeePosition), ErrorMessage = "Valor inválido para o cargo")]
    public EmployeePosition Position { get; set; }
}
