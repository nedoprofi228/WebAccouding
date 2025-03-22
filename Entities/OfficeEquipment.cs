namespace accoudingWeb.Entities;

public class OfficeEquipment : BaseItem<OfficeEquipment>
{
    public string PrinterType { get; set; } = String.Empty;
    public int Count { get; set; } = 0;

}