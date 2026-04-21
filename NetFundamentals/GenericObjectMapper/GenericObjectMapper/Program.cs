namespace GenericObjectMapper;

class Program
{
    public static void Main()
    {
        var employee = new Employee(1, "Alex", 5000, "IT");
        Console.WriteLine(employee);
        EmployeeDto employeeDto = ObjectMapper.Map<Employee, EmployeeDto>(employee);
        Console.WriteLine(employeeDto);
    }
}

