using Medic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Medic.DataAccess.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Hospital> Hospitals { get; set; }
    
    public DbSet<Report> Reports { get; set; } 
    
    public DbSet<Book> Bookings { get; set; }
    
    public DbSet<Message> Messages { get; set; }
    
    public DbSet<Feedback> Feedbacks { get; set; }
    
    public DbSet<Doctor> Doctors { get; set; }
    
    public DbSet<Attachment> Attachments { get; set; }
}