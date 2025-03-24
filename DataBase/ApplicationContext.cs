using accoudingWeb.Entities;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.DataBase;

public class ApplicationContext : DbContext
{
    // таблица юзеров
    public DbSet<Employee> Users { get; set; }
        
    // таблица организаций
    public DbSet<Organization> Organizations { get; set; }
    
    //таблицы для хранения заявок
    public DbSet<Ticket<Equipment>> TicketsEquipment { get; set; }
    public DbSet<Ticket<OfficeEquipment>> TicketsOfficeEquipment { get; set; }
    public DbSet<Ticket<PreciousMetals>> TicketsPreciousMetals { get; set; }
    
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
        modelBuilder.Entity<Employee>().HasOne(e => e.Organization).WithMany(o => o.Employeeres);
        // связи многие ко многим между организацией и предметами
        modelBuilder.Entity<Organization>().HasMany(o => o.Equipments).WithMany(p => p.Organizations);
        modelBuilder.Entity<Organization>().HasMany(o => o.PreciousMetals).WithMany(p => p.Organizations);
        modelBuilder.Entity<Organization>().HasMany(o => o.OfficeEquipments).WithMany(p => p.Organizations);
        
        // связи один ко многим между организацией и отчетами
        modelBuilder.Entity<Organization>().HasMany(o => o.AccoudingsEquipment).WithOne(p => p.Organization);
        modelBuilder.Entity<Organization>().HasMany(o => o.AccoudingsOfficeEquipment).WithOne(p => p.Organization);
        modelBuilder.Entity<Organization>().HasMany(o => o.AccoudingsPreciousMetals).WithOne(p => p.Organization);
        
        // связи многие ко многим между отчетами и предметами
        modelBuilder.Entity<Ticket<Equipment>>().HasMany(a => a.Items).WithMany(b => b.Tickets);
        modelBuilder.Entity<Ticket<OfficeEquipment>>().HasMany(a => a.Items).WithMany(b => b.Tickets);
        modelBuilder.Entity<Ticket<PreciousMetals>>().HasMany(a => a.Items).WithMany(b => b.Tickets);
        
        base.OnModelCreating(modelBuilder);
    }
}

