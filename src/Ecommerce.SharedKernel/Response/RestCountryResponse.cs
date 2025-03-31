namespace Ecommerce.SharedKernel.Response;

public class RestCountryResponse
{
    public Name Name { get; set; } = null!;
}

public partial class Name
{
    public string Common { get; set; } = string.Empty;

    public string Official { get; set; } = string.Empty;

    public Dictionary<string, NativeName> NativeName { get; set; } = null!;
}

public class NativeName
{
    public string Official { get; set; } = string.Empty;

    public string Common { get; set; } = string.Empty;
}
