using accoudingWeb.DataBase;
using accoudingWeb.Entities;

namespace accoudingWeb.Services;

public class AccoudingService
{
    private ApplicationContext _dbContext = ApplicationContext.Instance();

    // дженерик метод для создания отчетов разных предметов
    private Accouding<T> CreateAccouding<T>(string name, AccoudingStatus status, List<T> items, ItemStatus itemStatus,
        decimal price = -1) where T : BaseItem<T>
    {
        Accouding<T> accouding = new Accouding<T>();
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
    public Accouding<T> CreateBuyAccouding<T>(string name, List<T> items, decimal price = -1) where T : BaseItem<T>
        => CreateAccouding<T>(name, AccoudingStatus.Pending, items, ItemStatus.Buy, price);
    
    // дженерик метод для создания отчета починки разных предметов
    public Accouding<T> CreateRepairAccouding<T>(string name, List<T> items, decimal price = -1) where T : BaseItem<T> 
        => CreateAccouding<T>(name, AccoudingStatus.Pending, items, ItemStatus.Repair, price);
    
    // дженерик метод для создания отчета списания разных предметов
    public Accouding<T> CreateWriteOfAccouding<T>(string name, List<T> items, decimal price = -1) where T : BaseItem<T> 
        => CreateAccouding<T>(name, AccoudingStatus.Pending, items, ItemStatus.WriteOf, price);

    public Accouding<T> AcceptAccouding<T>(Accouding<T> accouding) where T : BaseItem<T>
    {
        accouding.AccoudingStatus = AccoudingStatus.Accepted;
        return accouding;
    }

    public Accouding<T> DeclineAccouding<T>(Accouding<T> accouding) where T : BaseItem<T>
    {
        accouding.AccoudingStatus = AccoudingStatus.Declined;
        return accouding;
    }
}