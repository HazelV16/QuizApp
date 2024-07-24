using Microsoft.EntityFrameworkCore;
using QuizApp.Data.Entities;

namespace QuizApp.Data;

public class QuizContext : DbContext
{
    public QuizContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>()
            .Property(q => q.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Answer>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd();
    }
}