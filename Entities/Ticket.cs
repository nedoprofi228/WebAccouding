using Microsoft.AspNetCore.Http.HttpResults;

namespace accoudingWeb.Entities;

public class Ticket<T>
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string ItemStatus { get; set; }
    public string TicketStatus { get; set; }
    public List<T> Items { get; set; } = [];
    public decimal TotalPrice { get; set; } = 0;
    
    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
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