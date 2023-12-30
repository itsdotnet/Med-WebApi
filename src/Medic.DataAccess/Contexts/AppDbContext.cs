using Medic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Medic.DataAccess.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Feedback> Feedbacks { get; set; }
    
    public DbSet<Doctor> Doctors { get; set; }
    
    public DbSet<Attachment> Attachments { get; set; }
}