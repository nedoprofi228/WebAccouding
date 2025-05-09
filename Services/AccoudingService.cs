using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.ViewModels;

namespace accoudingWeb.Services;

/// <summary>
/// сервис для создания и получения отчетов
/// </summary>
/// <param name="dbContext"></param>
public class AccoudingService(ApplicationContext dbContext)
{
    
    // дженерик метод для создания отчетов разных предметов
    public Accouding<T> CreateAccouding<T>(Employee manager, Ticket<T> ticket) 
        
    {
        Accouding<T> accouding = new Accouding<T>
        {
            Date = DateTime.Now,
            Ticket = ticket,
            Manager = manager,
        };

        return accouding;
    }

    public Accouding<T>? GetById<T>(long id)=>
        dbContext.Set<Accouding<T>>().Find(id);
    
    public List<Accouding<T>> Get<T>() =>
        dbContext.Set<Accouding<T>>().ToList();
}