namespace Medic.Service.DTOs.Users;

public class UserUpdateDto
{
    public long Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime DateOfBirth { get; set; }
}