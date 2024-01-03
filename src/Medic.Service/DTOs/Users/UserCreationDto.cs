namespace Medic.Service.DTOs.Users;

public class UserCreationDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DateOfBirth { get; set; }
}