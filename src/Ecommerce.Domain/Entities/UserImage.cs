using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class UserImage : IntermediaryEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public int ImageId { get; set; }

    public Image Image { get; set; } = null!;

    public bool IsActive { get; set; }
}
