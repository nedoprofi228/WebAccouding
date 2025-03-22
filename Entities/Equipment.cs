namespace accoudingWeb.Entities;

public class Equipment : BaseItem<Equipment>
{
    public string Type { get; set; } = String.Empty;
    public int Count { get; set; } = 0;
    
}