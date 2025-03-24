using Microsoft.AspNetCore.Http.HttpResults;

namespace accoudingWeb.Entities;

public class Ticket<T>
{
    public int Id {get; set;}
    public string Name {get; set;}
    public ItemStatus ItemStatus { get; set; }
    public Ticketstatus Ticketstatus { get; set; } = Ticketstatus.Pending;
    public List<T> Items { get; set; } = [];
    public decimal TotalPrice { get; set; } = 0;
    
    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    public long OrganizationId { get; set; }
    public Organization Organization { get; set; }
}

public enum Ticketstatus
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