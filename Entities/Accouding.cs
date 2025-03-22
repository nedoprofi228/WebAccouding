namespace accoudingWeb.Entities;

public class Accouding<T>
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
    public decimal TotalPrice { get; set; } = decimal.Zero;
    public ItemStatus ItemStatus { get; set; }
    public AccoudingStatus AccoudingStatus { get; set; }
    
    public long SemderId { get; set; }
    public Employee Sender { get; set; }
    
    public long ManagerId { get; set; }
    public Employee Manager { get; set; }
    
    public long OrganizationId { get; set; }
    public Organization Organization { get; set; }
    public List<T> Items { get; set; } = [];
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