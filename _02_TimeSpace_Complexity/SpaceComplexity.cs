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
}