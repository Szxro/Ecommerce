using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce.SharedKernel.Contracts;

public interface IQuery<out TResponse> : IRequest<TResponse> { }

public interface ICachedQuery<out TResponse> : IQuery<TResponse>, ICachedQuery { }

public interface ICachedQuery
{
    string CachedKey { get; }

    MemoryCacheEntryOptions? Options { get; }
}