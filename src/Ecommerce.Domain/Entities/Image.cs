using Ecommerce.SharedKernel.Common;

namespace Ecommerce.Domain.Entities;

public class Image : Entity
{
    public Image()
    {
        UserImages = new HashSet<UserImage>();
    }

    public string FileName { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public int TotalSize { get; set; }

    public string Format { get; set; } = string.Empty;

    public int Height { get; set; }

    public int Width { get; set; }

    public ICollection<UserImage> UserImages { get; set; }
}
