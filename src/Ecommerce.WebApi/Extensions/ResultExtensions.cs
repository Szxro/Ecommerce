using Ecommerce.SharedKernel.Common.Primitives;

namespace Ecommerce.WebApi.Extensions;

public static partial class ApiExtensions
{
    public static TOut Match<TOut>(
       this Result result,
       Func<TOut> onSuccess,
       Func<Result, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static TOut Match<TValue, TOut>(
        this Result<TValue> result,
        Func<TValue, TOut> onSuccess,
        Func<Result<TValue>, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}