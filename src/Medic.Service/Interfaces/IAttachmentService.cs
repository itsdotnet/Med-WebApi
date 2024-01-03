using Medic.Domain.Entities;
using Medic.Service.DTOs.Attachments;

namespace Medic.Service.Interfaces;

public interface IAttachmentService
{
    Task<bool> DeleteAsync(long id);
    Task<Attachment> UploadAsync(AttachmentCreationDto dto);
}