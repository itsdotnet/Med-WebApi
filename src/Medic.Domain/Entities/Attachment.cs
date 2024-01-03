using Medic.Domain.Commons;

namespace Medic.Domain.Entities;

public class Attachment : Auditable
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
}