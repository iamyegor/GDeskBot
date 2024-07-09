using Domain.Common;

namespace Domain.Errors;

public class Error : ValueObject
{
    public string Code { get; set; }
    public IDictionary<string, object?> Details { get; set; }

    public Error(string code, IDictionary<string, object?>? details = null)
    {
        Code = code;
        Details = details ?? new Dictionary<string, object?>();
    }

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Code;
    }
}
