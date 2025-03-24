using System.ComponentModel.DataAnnotations;
using accoudingWeb.Entities;

namespace accoudingWeb.ViewModels;

public class TicketModel<T>
{
    [Required]
    public string Name { get; set; } 
    [Required]
    public List<T> Items { get; set; }
    public decimal TotalPrice { get; set; } = -1;
    public ItemStatus ItemStatus { get; set; }

}