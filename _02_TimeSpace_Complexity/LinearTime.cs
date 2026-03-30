using static System.Console; 

namespace _02_TimeSpace_Complexity; 

public class LinearTime 
{
    public string? FindFirstError(string[] logs) 
    {
        foreach (var log in logs)   // visits upto 'n' times
        {
            if (log.StartsWith("ERROR")) 
                return log; 
        } 
        return null; 
    }
}