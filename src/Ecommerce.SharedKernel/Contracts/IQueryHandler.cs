using MediatR;

namespace Ecommerce.SharedKernel.Contracts;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{ }