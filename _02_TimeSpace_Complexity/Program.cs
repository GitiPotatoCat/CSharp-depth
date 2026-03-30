using static System.Console; 

namespace _02_TimeSpace_Complexity; 


public class Program 
{
    static void Main() 
    {
        ConstantTime cT = new(); 
        LinearTime lT = new(); 

        cT.SessionCacheExample();
        
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
    }
}