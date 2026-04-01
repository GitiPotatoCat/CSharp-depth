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



    // Scenario: An event venue needs a seating chart. 
    //  Given n rows and n seats per row, 
    //  we build a full 2D grid to track every seat -> O(n^2) Space 

    public void QuadraticSpaceExample() 
    {
        int n = 4;      // 4 rows x 4 seats = 16 seats 

        WriteLine($"Building seating-chart for {n} rows x {n} seats:"); 
        WriteLine(new string('-', 45)); 

        string[,] seatingChart = BuildingSeatingChart(n); 

        PrintSeatingChart(seatingChart, n); 

        WriteLine(new string('-', 45)); 
        WriteLine($"Total seats in memory: {n} x {n} = {n * n} (n²) = {n}² = {n * n}");
    } 

    // O(n²) Space 
    private string[,] BuildingSeatingChart(int n) 
    {
        var seatingChart = new string[n, n]; 

        for (int row = 0; row < n; row++) 
        {
            for (int seat = 0; seat < n; seat++) 
            {
                // Each cell occupies a slot in memory 
                seatingChart[row, seat] = $"G{row + 1}{seat + 1}"; 
            }
        }

        return seatingChart; 
    }


    // Helper: print the seating chart grid 
    private void PrintSeatingChart(string[,] chart, int n) 
    {
        WriteLine("     Seat1       Seat2       Seat3       Seat4"); 
        for (int row = 0; row < n; row++) 
        {
            Write($"Row {row + 1} | "); 
            for (int seat = 0; seat < n; seat++) 
            {
                Write($"{chart[row, seat], -8}"); 
            }
            WriteLine();
        }
    } 



    // Scenario: A sorted dictionary has thousands of words. 
    //  We use RECURSIVE Binary Search to find a word. 
    //  Each recursive call adds 1 stack frame to memory + O(log n) Space 
    public void LogarithmicSpaceExample() 
    {
        var dictionary = new List<string> 
        {
            "Apple", 
            "Banana", 
            "Cherry", 
            "Dragon", 
            "Elephant", 
            "Falcon", 
            "Grape", 
            "Horizon", 
            "Igloo", 
            "Jungle" 
        };

        var targetWord = "Falcon"; 

        WriteLine($"Searching dictionary for: {targetWord}"); 
        WriteLine(new string('-', 45)); 

        int resultIndex = FindWord(dictionary, targetWord, 0, dictionary.Count - 1, 1); 

        if (resultIndex != -1) WriteLine($"\n '{targetWord}' found at position {resultIndex + 1} in dictionary."); 
        else WriteLine($"\n '{targetWord}' not found in dictionary."); 

        WriteLine(new string('-', 45)); 
        WriteLine($"Max stack frames used: -{(int)Math.Ceiling(Math.Log(dictionary.Count))} (log2 {dictionary.Count} = {Math.Log(dictionary.Count):F1})"); 
    } 

    // O(log n) Space - each recursive call adds 1 frame to the call stack 
    // stack depth never exceeds log2(n) frames at any point 
    private int FindWord(
        List<string> dictionary, 
        string targetWord, 
        int left, 
        int right, 
        int depth
    ) 
    {
        // Base case: search space exhausted -> pop stack frame 
        if (left > right) return -1; 

        int mid = left + (right - left) / 2; 

        WriteLine($"    Stack Frame {depth}: checking '{dictionary[mid]}' (left={left}, right={right})"); 

        int comparison = string.Compare(dictionary[mid], targetWord, StringComparison.Ordinal); 

        if (comparison == 0) return mid; 
        else if (comparison < 0) {
            WriteLine($"    {targetWord} comes AFTER -> go RIGHT"); 
            return FindWord(dictionary, targetWord, mid+1, right, depth+1); 
        } else {
            WriteLine($"    '{targetWord}' comes BEFORE -> go LEFT"); 
            return FindWord(dictionary, targetWord, left, mid -1, depth + 1);
        }
    }
}