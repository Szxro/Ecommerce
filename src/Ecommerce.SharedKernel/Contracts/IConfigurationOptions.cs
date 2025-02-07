namespace Ecommerce.SharedKernel.Contracts;

/// <summary>
/// Marker interface for the options classes
/// </summary>
public interface IConfigurationOptions
{
    public string SectionName { get; }
}
