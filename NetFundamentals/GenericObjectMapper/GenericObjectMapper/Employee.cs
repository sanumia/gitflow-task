namespace GenericObjectMapper;

public class Employee(
    int id, 
    string name, 
    decimal salary, 
    string department)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public decimal Salary { get; set; } = salary;
    public string Department { get; set; } = department;

    public Employee() : this(0, "", 0m, "")
    {
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Salary: {Salary}, Department: {Department}";
    }
}
