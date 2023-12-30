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
        MassageRepository = new Repository<Massage>(_dbContext);
        AttachmentRepository = new Repository<Attachment>(_dbContext);
    }

    public IRepository<Doctor> DoctorRepository { get; }
    public IRepository<User> UserRepository { get; }
    public IRepository<Feedback> FeedbackRepository { get; }
    public IRepository<Massage> MassageRepository { get; }
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