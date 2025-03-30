using accoudingWeb.Entities;

namespace accoudingWeb.DTO;

public class AccoudingDto
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public Employee Manager { get; set; }
    public TicketDto Ticket { get; set; }
}