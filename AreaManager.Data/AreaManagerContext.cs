using AreaManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AreaManager.Data;

public class AreaManagerContext : DbContext
{
    public AreaManagerContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AreaData> AreaDatas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AreaData>()
            .HasKey(l => l.Id);

        base.OnModelCreating(modelBuilder);
    }
}