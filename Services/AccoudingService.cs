using accoudingWeb.DataBase;
using accoudingWeb.Entities;

namespace accoudingWeb.Services;

public class AccoudingService(ApplicationContext dbContext)
{
    
    // дженерик метод для создания отчетов разных предметов
    private Accouding<T> CreateAccouding<T>(Employee sender,
        string name, 
        AccoudingStatus status, 
        List<T> items,
        ItemStatus itemStatus,
        decimal price = -1) where T : BaseItem<T>
    {
        Accouding<T> accouding = new Accouding<T>
        {
            Sender = sender,
            Name = name,
            AccoudingStatus = status,
            Date = DateTime.Now,
            Items = items,
            ItemStatus = itemStatus,
            Organization = sender.Organization
        };
        
        accouding.Name = name;
        accouding.Date = DateTime.Now;
        accouding.AccoudingStatus = status;
        accouding.Items = items;
        accouding.ItemStatus = itemStatus;
        accouding.TotalPrice = items.Sum(item => item.Price);

        if (price > -1)
            accouding.TotalPrice = price;

        return accouding;
    }

    // дженерик метод для создания отчета покупки разных предметов
    public Accouding<T> CreateBuyAccouding<T>(Employee sender, string name, List<T> items, decimal price = -1) where T : BaseItem<T>
        => CreateAccouding<T>(sender, name, AccoudingStatus.Pending, items, ItemStatus.Buy, price);
    
    // дженерик метод для создания отчета починки разных предметов
    public Accouding<T> CreateRepairAccouding<T>(Employee sender, string name, List<T> items, decimal price = -1) where T : BaseItem<T> 
        => CreateAccouding<T>(sender, name, AccoudingStatus.Pending, items, ItemStatus.Repair, price);
    
    // дженерик метод для создания отчета списания разных предметов
    public Accouding<T> CreateWriteOfAccouding<T>(Employee sender, string name, List<T> items, decimal price = -1) where T : BaseItem<T> 
        => CreateAccouding<T>(sender, name, AccoudingStatus.Pending, items, ItemStatus.WriteOf, price);

    public Accouding<T> AcceptAccouding<T>(Employee manager, Accouding<T> accouding) where T : BaseItem<T>
    {
        accouding.AccoudingStatus = AccoudingStatus.Accepted;
        accouding.Manager = manager;
        return accouding;
    }

    public Accouding<T> DeclineAccouding<T>(Employee manager, Accouding<T> accouding) where T : BaseItem<T>
    {
        accouding.AccoudingStatus = AccoudingStatus.Declined;
        accouding.Manager = manager;
        return accouding;
    }

    public Accouding<T>? GetById<T>(long id) where T : BaseItem<T> =>
        dbContext.Set<Accouding<T>>().Find(id);
    
    public List<Accouding<T>> Get<T>() where T : BaseItem<T> =>
        dbContext.Set<Accouding<T>>().ToList();
}