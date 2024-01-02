using Microsoft.AspNetCore.Http;

namespace Medic.Service.DTOs.Attachments;

public class AttachmentCreationDto
{
    public IFormFile File { get; set; }
}