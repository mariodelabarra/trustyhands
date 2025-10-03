using TrustyHands.Platform.Professionals.API.Shared.Models.Enums;

namespace TrustyHands.Platform.Professionals.API.Shared.Models;

public class Professional : BaseEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public Specialty Specialty { get; set; } = Specialty.None;
}
