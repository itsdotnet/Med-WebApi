using Medic.Domain.Enums;

namespace Medic.Service.DTOs.Users;

public class UserResultDto
{
    public long Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public UserRole UserRole { get; set; }
    public DateTime DateOfBirth { get; set; }
}