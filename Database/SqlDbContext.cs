using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<InputHtmlFileEntity> InputHtmlFiles { get; set; } = null!;
    public DbSet<OutputPdfFileEntity> OutputPdfFiles { get; set; } = null!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
           optionsBuilder.UseSqlite("Data Source=file_board.db");
        }
    }
}