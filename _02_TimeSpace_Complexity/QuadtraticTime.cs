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



    public void QTExample2() 
    {
        var products = new List<(string prodName, double prodPrice)> 
        {
            ("Apple",   16.50), 
            ("Bread",   18.19), 
            ("Milk",    30.05), 
            ("Cheese",  25.65), 
            ("Butter",  42.85), 
            ("Eggs",    12.39), 
        }; 

        double budgetRange = 20.00; 

        WriteLine($"Finding product pairs within inr{budgetRange:F2} price range."); 
        WriteLine(new string('-', 50)); 

        FindSimilarPricedProduct(products, budgetRange); 
    } 


    private void FindSimilarPricedProduct(List<(string Name, double Price)> products, double budgetRange) 
    {
        int pairCount = 0; 

        for (int i = 0; i < products.Count; i++) 
        {
            for (int j = i + 1; j < products.Count; j++) 
            {
                double priceDiff = Math.Abs(products[i].Price - products[j].Price); 

                if (priceDiff <= budgetRange) 
                {
                    WriteLine($@"
                        {products[i].Name}: {products[i].Price:F2}
                        {products[i].Name}: {products[i].Price:F2} 
                        Difference: {priceDiff}
                    "); 

                    pairCount++;
                }
            }
        } 

        WriteLine(new string('-', 50)); 
        WriteLine($"Total Similar priced pairs: {pairCount}");
    }
}