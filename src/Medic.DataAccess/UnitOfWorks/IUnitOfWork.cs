using Medic.DataAccess.Repositories;
using Medic.Domain.Entities;

namespace Medic.DataAccess.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    IRepository<Doctor> DoctorRepository { get; }
    
    IRepository<User> UserRepository { get; }
    
    IRepository<Hospital> HospitalRepository { get; }
    
    IRepository<Book> BookRepository { get; }
    
    IRepository<Report> ReportRepository { get; }
    
    IRepository<Feedback> FeedbackRepository { get; }
    
    IRepository<Message> MassageRepository { get; }
    
    IRepository<Attachment> AttachmentRepository { get; }
    
    Task<bool> SaveAsync();
}