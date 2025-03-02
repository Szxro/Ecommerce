using Ecommerce.Infrastructure.Common;
using Ecommerce.SharedKernel.Contracts;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure.Options.Hash;

public class HashOptionsSetup : BaseOptions<HashOptions>
{
    public HashOptionsSetup(IConfiguration configuration) : base(configuration) { }
}

public class HashOptions : IConfigurationOptions
{
    public string SectionName => "HashingOptions";

    public int SaltSize { get; set; }

    public int HashSize { get; set; }

    public int Iterations { get; set; }
}