using MediatR;

namespace Ecommerce.SharedKernel.Contracts;

public interface IQuery<out TResponse> : IRequest<TResponse> { }