using Medic.Domain.Constants;

namespace Medic.Domain.Commons;

public class Auditable
{
    public long Id { get; set; }

    public bool IsDeleted { get; set; }
    
    public DateTime CreatedAt { get; set; } = TimeConstants.GetNow();
    
    public DateTime UpdatedAt { get; set; }
}