namespace accoudingWeb.Entities;

public class Accouding<T>
{
    public long Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    
    public long TicketId { get; set; }
    public Ticket<T> Ticket { get; set; }
    
    public long ManagerId { get; set; }
    public Employee Manager { get; set; }
    public Accouding(){ }

    public Accouding(Accouding<T> item)
    {
        Date = item.Date;
        ManagerId = item.ManagerId;
        Manager = item.Manager;
    }
}

