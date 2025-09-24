using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Pagination;

public enum OrderByDirection
{
    [Description("DESC")]
    DESCENDING = 1,
    [Description("ASC")]
    ASCENDING = 2
}
