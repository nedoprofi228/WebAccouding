namespace accoudingWeb.Entities;

public class BaseItem<T>
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public decimal Price { get; set; } = 0;
    
    // кол-во текущих предметов
    public int Count { get; set; } = 0;
    
    public List<Organization> Organizations { get; set; } = [];
    public List<Ticket<T>> Tickets { get; set; } = [];
}