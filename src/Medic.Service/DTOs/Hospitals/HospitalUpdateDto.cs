namespace Medic.Service.DTOs.Hospitals;

public class HospitalUpdateDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public string ContactNumber { get; set; }

    public int Capacity { get; set; }
}