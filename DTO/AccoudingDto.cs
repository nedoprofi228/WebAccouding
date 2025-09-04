using accoudingWeb.Entities;

namespace accoudingWeb.DTO;

public class AccoudingDto<T>
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public Employee Manager { get; set; }
    public TicketDto<T> Ticket { get; set; }
}