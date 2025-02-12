using Ecommerce.Domain.Contracts;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Ecommerce.Application.Common.Pipelines;

public class RequestTransactionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand 
    where TResponse : Result
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RequestTransactionHandlingBehavior<TRequest, TResponse>> _logger;

    public RequestTransactionHandlingBehavior(
        IUnitOfWork unitOfWork,
        ILogger<RequestTransactionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // if the transaction is not commit, when the object is disposed it's going to be automatically rollback thanks to the using statement
        using IDbTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        string commandName = typeof(TRequest).Name;

        try
        {
            TResponse response = await next();

            _logger.LogInformation(
                "The command {commandName} was completed succesfully, committing the transaction.",
                commandName);

            transaction.Commit();

            return response;

        }
        catch
        {
            _logger.LogError(
                "An unexpected error happen while trying to complete the command {commandName}, rollying back the transaction.",
                commandName);

            transaction.Rollback();

            throw;
        }
    }
}
