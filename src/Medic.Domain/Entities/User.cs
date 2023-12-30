using Medic.Domain.Commons;
using Medic.Domain.Enums;

namespace Medic.Domain.Entities;

public class User : Auditable
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DateOfBirth { get; set; }
    public UserRole UserRole { get; set; }

    public long? AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}