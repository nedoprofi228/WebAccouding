namespace accoudingWeb.Entities;

public class Employee
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = String.Empty;
    public Role Role { get; set; } = Role.Employee;
    
    public long OrganizationId { get; set; }
    public Organization Organization { get; set; }
}

public enum Role
{
    Employee,
    Manager,
    Admin
}