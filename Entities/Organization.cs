namespace accoudingWeb.Entities;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    
    // предметы которые имеются в организации
    public List<Equipment> Equipments { get; set; }
    public List<OfficeEquipment> OfficeEquipments { get; set; }
    public List<PreciousMetals> PreciousMetals { get; set; }
    
    // отчеты организации
    public List<Accouding<Equipment>> AccoudingsEquipment { get; set; }
    public List<Accouding<OfficeEquipment>> AccoudingsOfficeEquipment { get; set; }
    public List<Accouding<PreciousMetals>> AccoudingsPreciousMetals { get; set; }
}