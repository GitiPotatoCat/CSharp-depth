using static System.Console; 

namespace _02_TimeSpace_Complexity; 

public class QuadraticTime 
{
    public void QTExample() 
    { 
        // Scenario: Finding the exact server name from a large pool of servers where each server has 1 to n number of jobs
        var jobName = "C# docker auth running"; 

        var servers = new List<List<string>> 
        {
            new List<string> { "Python backup job running ",    "Node Auth Service",    "Java DB Migrator" }, 
            new List<string> { "GO API Gateway",    "C# Arc CPU x86 running ",  "C# docker auth running" }, 
            new List<string> { "Rust log running",  "PHP mail sender",  "TypeScript event listener" }
        }; 


        string? foundOnServer = FindJobOnServer(servers, jobName); 


        if (foundOnServer != null) WriteLine($"Job {jobName} found on {foundOnServer}"); 
        else WriteLine($"Job Name {jobName} is not found on server");

    } 


    private string? FindJobOnServer(List<List<string>> serverPool, string targetJob) 
    {
        for (int i = 0; i < serverPool.Count; i++) 
        {
            for (int j = 0; j < serverPool[i].Count; j++) 
            {
                if (serverPool[i][j] == targetJob) 
                    return $"Server found at id: {i} job running id: {j}";
            }
        }

        return null; 
    }
}