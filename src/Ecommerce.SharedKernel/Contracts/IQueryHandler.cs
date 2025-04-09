using Ecommerce.SharedKernel.Common.Primitives;
using MediatR;

namespace Ecommerce.SharedKernel.Contracts;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }