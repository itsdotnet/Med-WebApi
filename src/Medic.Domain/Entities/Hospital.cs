using Medic.Domain.Commons;

namespace Medic.Domain.Entities;

public class Hospital : Auditable
{
    public string Name { get; set; }

    public string Location { get; set; }

    public string ContactNumber { get; set; }

    public bool IsOpen { get; set; }
    
    public int Capacity { get; set; }

    public ICollection<Doctor> Doctors { get; set; }
}