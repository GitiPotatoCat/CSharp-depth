using static System.Console; 
using System.Collections.Generic; 

namespace _02_TimeSpace_Complexity; 


public class ConstantTime 
{
    public void SessionCacheExample() 
    {
        // Scenario: Looking up a user session ID in a cache 
        // No matter how many session exist, this is one operation. 
        var sessionCache = new Dictionary<string, string> 
        {
            ["abc123"]="user:42", 
            ["xyz789"]="user:17" 
        }; 

        string GetSession(string token) => sessionCache[token];     // O(1)  

        WriteLine(GetSession("abc123"));
    }
}