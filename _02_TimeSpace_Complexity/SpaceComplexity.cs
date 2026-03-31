using static System.Console; 

namespace _02_TimeSpace_Complexity; 

public class SpaceComplexity 
{
    // Scenario: A company has a list of employee salaris. 
    // Find the MINIMUM and MAXIMUM salary 
    // without creating any extra collections -> O(1) Space 
    public void ConstantSpaceExample() 
    {
        var salaries = new List<double> 
        {
            45000.00, 
            62000.00, 
            38000.00, 
            95000.00, 
            71000.00, 
            55000.00, 
            48000.00
        }; 

        WriteLine("Employee Salary Analysis:"); 
        WriteLine(new string('-', 40)); 

        FindMinMaxSalary(salaries); 
    } 


    // O(1) Space - only 2 fixed variables used (miinSalary, maxSalary) 
    // regardless of how many employees exist, memory never grows 
    private void FindMinMaxSalary(List<double> salaries) 
    {
        double minSalary = salaries[0]; 
        double maxSalary = salaries[0]; 

        // O(n) time, still O(1) Space 
        for (int i=1; i<salaries.Count; i++) 
        {
            if (salaries[i] < minSalary) minSalary = salaries[i]; 
            else maxSalary = salaries[i];
        } 

        WriteLine($"Lowest Salary: {minSalary:N2}"); 
        WriteLine($"Highest Salary: {maxSalary:N2}"); 
        WriteLine($"Salary Range: {(maxSalary - minSalary):N2}"); 
    } 


    // Scenario: A company has a list of employees with salaries. 
    //  Filter out ONLY the high earning employees (above threshold) 
    //  into a NEW list -> O(n) Space 
    public void LinearSpaceExample() 
    {
        var employees = new List<(string Name, double Salary)> 
        {
            ("Anil", 45000.00), 
            ("Binod", 82000.00), 
            ("Chachundar", 38000.00), 
            ("Dravir", 95000.00), 
            ("Eve", 71000), 
            ("Frank", 55000.00), 
            ("Golu", 88000.00)
        }; 

        double threshold = 70000.00; 

        WriteLine($"Filtering employees with salary above {threshold:N2}"); 
        WriteLine(new string('-', 50)); 

        var highEarners = FilterHighEarners(employees, threshold); 

        foreach (var emp in highEarners) WriteLine($"{emp.Name, -10} -> {emp.Salary:N2}"); 

        WriteLine(new string('-', 50)); 
        WriteLine($"Total high earners found: {highEarners.Count} out of {employees.Count}"); 
    } 


    // O(n) Space - a NEW list is created that can grow 
    // up to the same size as the input list 
    private List<(string Name, double Salary)> FilterHighEarners(
        List<(string Name, double Salary)> employees, 
        double threshold
    ) 
    {
        var highEarners = new List<(string Name, double Salary)>(); 

        for (int i=0; i<employees.Count; i++) 
        {
            if (employees[i].Salary > threshold) highEarners.Add(employees[i]); 
        } 

        return highEarners; 
    }
}