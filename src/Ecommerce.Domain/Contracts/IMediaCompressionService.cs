using Ecommerce.Domain.Results;
using Ecommerce.SharedKernel.Common.Primitives;
using Ecommerce.SharedKernel.Enums;

namespace Ecommerce.Domain.Contracts;

public interface IMediaCompressionService
{
    Result<CompressionResult> ImageCompressionAndSave(Stream imageStream, long fileLength, string extension, ImageQuality quality);
}
