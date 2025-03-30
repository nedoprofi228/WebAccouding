namespace accoudingWeb.DTO;

public class TicketDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ItemStatus { get; set; }
    public string TicketStatus { get; set; }
    public decimal TotalPrice { get; set; }
    public string EmployeeName { get; set; }
    public List<MinimalItem> Items { get; set; }
}