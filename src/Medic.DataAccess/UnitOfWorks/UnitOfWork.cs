using Medic.DataAccess.Contexts;
using Medic.DataAccess.Repositories;
using Medic.Domain.Entities;

namespace Medic.DataAccess.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        
        DoctorRepository = new Repository<Doctor>(_dbContext);
        UserRepository = new Repository<User>(_dbContext);
        FeedbackRepository = new Repository<Feedback>(_dbContext);
        MessageRepository = new Repository<Message>(_dbContext);
        AttachmentRepository = new Repository<Attachment>(_dbContext);
        HospitalRepository = new Repository<Hospital>(_dbContext);
        BookRepository = new Repository<Book>(_dbContext);
        ReportRepository = new Repository<Report>(_dbContext);
    }

    public IRepository<Doctor> DoctorRepository { get; }
    public IRepository<User> UserRepository { get; }
    public IRepository<Hospital> HospitalRepository { get; }
    public IRepository<Book> BookRepository { get; }
    public IRepository<Report> ReportRepository { get; }
    public IRepository<Feedback> FeedbackRepository { get; }
    public IRepository<Message> MessageRepository { get; }
    public IRepository<Attachment> AttachmentRepository { get; }
    
    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }

    public async Task<bool> SaveAsync()
    {
        var saved = await _dbContext.SaveChangesAsync();
        return saved > 0;
    }
}