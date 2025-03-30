using System.ComponentModel.DataAnnotations;
using accoudingWeb.Entities;

namespace accoudingWeb.ViewModels;

public class TicketModel<T>
{
    [Required]
    public string Name { get; set; } 
    [Required]
    public List<long> ItemsId { get; set; }
    public decimal TotalPrice { get; set; } = -1;
    public string ItemStatus { get; set; }

}