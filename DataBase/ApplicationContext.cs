using accoudingWeb.Entities;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.DataBase;

public class ApplicationContext : DbContext
{
    // таблица юзеров
    public DbSet<User> Users { get; set; }
        
    // таблица организаций
    public DbSet<Organization> Organizations { get; set; }
    
    // таблицы для храниния отчетов
    public DbSet<Accouding<Equipment>> AccoudingsEquipment { get; set; }
    public DbSet<Accouding<OfficeEquipment>> AccoudingsOfficeEquipment { get; set; }
    public DbSet<Accouding<PreciousMetals>> AccoudingsPreciousMetals { get; set; }
    
    // таблицы существующих предметов
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<OfficeEquipment> OfficeEquipments { get; set; }
    public DbSet<PreciousMetals> PreciousMetals { get; set; }
    
    // указана строка подключения
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=database.db");
    }

    // настраиваем связи между таблицами
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // связи многие ко многим между организацией и предметами
        modelBuilder.Entity<Organization>().HasMany(o => o.Equipments).WithMany(p => p.Organizations);
        modelBuilder.Entity<Organization>().HasMany(o => o.PreciousMetals).WithMany(p => p.Organizations);
        modelBuilder.Entity<Organization>().HasMany(o => o.OfficeEquipments).WithMany(p => p.Organizations);
        
        // связи многие ко многим между организацией и отчетами
        modelBuilder.Entity<Organization>().HasMany(o => o.AccoudingsEquipment).WithMany(p => p.Organizations);
        modelBuilder.Entity<Organization>().HasMany(o => o.AccoudingsOfficeEquipment).WithMany(p => p.Organizations);
        modelBuilder.Entity<Organization>().HasMany(o => o.AccoudingsPreciousMetals).WithMany(p => p.Organizations);
        
        // связи многие ко многим между отчетами и предметами
        modelBuilder.Entity<Accouding<Equipment>>().HasMany(a => a.Items).WithMany(b => b.Accoudings);
        modelBuilder.Entity<Accouding<OfficeEquipment>>().HasMany(a => a.Items).WithMany(b => b.Accoudings);
        modelBuilder.Entity<Accouding<PreciousMetals>>().HasMany(a => a.Items).WithMany(b => b.Accoudings);
        
        base.OnModelCreating(modelBuilder);
    }
}

