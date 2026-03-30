using static System.Console; 

namespace _02_TimeSpace_Complexity; 


public class Program 
{
    static void Main() 
    {
        ConstantTime cT = new(); 
        LinearTime lT = new(); 
        QuadraticTime qT = new(); 
        LogarithimicTime logT = new(); 

        WriteLine("\n---Constant Time O(1)---");
        cT.SessionCacheExample();
        
        WriteLine("\n---Liear Time O(n)---");
        var logs = new string[]
        {
            "INFO Application started",
            "INFO Processing request",
            "ERROR Database connection failed",
            "INFO Request completed"
        };
        string? result = lT.FindFirstError(logs);
        if (result != null) WriteLine($"First error found: {result}");
        else WriteLine("No errors found."); 

        WriteLine("\n---Quadratic Time (O^2)---"); 
        qT.QTExample(); 

        WriteLine("\n---Logarithmic Time O(log n)"); 
        logT.LogTExample(); 
    }
}