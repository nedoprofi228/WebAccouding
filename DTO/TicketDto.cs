namespace accoudingWeb.DTO;

public class TicketDto<T>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ItemStatus { get; set; }
    public string TicketStatus { get; set; }
    public decimal TotalPrice { get; set; }
    public string EmployeeName { get; set; }
    public List<T> Items { get; set; }
}