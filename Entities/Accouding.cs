namespace accoudingWeb.Entities;

public class Accouding<T>
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
    public decimal TotalPrice { get; set; } = decimal.Zero;
    public ItemStatus ItemStatus { get; set; }
    public AccoudingStatus AccoudingStatus { get; set; }
    public List<T> Items { get; set; } = [];
    public List<Organization> Organizations { get; set; } = [];
}

public enum AccoudingStatus
{
    Pending,
    Accepted,
    Declined,
}

public enum ItemStatus
{
    Repair,
    Buy,
    WriteOf
}