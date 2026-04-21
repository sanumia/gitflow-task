namespace GenericObjectMapper;

public class EmployeeDto(int id, string name, decimal salary)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public decimal Salary { get; set; } = salary;

    public EmployeeDto() : this(0, "", 0m)
    {
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Salary: {Salary}";
    }
}
