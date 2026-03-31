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
        ExponentialExample eT = new(); 

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
        WriteLine(new string('-', 20)); 
        qT.QTExample2(); 

        WriteLine("\n---Logarithmic Time O(log n)"); 
        logT.LogTExample(); 

        WriteLine("\n---Exponential Time O(2^n)---"); 
        eT.ExpoExample(); 

        WriteLine(new string('=', 45)); 
        WriteLine("\n\nSpace Complexity"); 
        SpaceComplexity spaceComplexity = new(); 

        WriteLine("---Constant Time---"); 
        spaceComplexity.ConstantSpaceExample();
    }
}