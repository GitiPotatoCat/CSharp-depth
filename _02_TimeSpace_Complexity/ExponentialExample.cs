using static System.Console; 

namespace _02_TimeSpace_Complexity; 


public class ExponentialExample 
{
    /*
        scenario: A manager has a list of tasks. 
        They want to see every possible subset of tasks 
        that can be assigned to a team. 
        Each task has 2 choices -> assign it or skip it (2ⁿ)
    */ 

    public void ExpoExample() 
    {
        var tasks = new List<string> 
        {
            "Angular", 
            ".NET", 
            "SQL"
        }; 

        WriteLine("All possible task assignment combinations:"); 
        WriteLine(new string('-', 45)); 

        var combinations = new List<List<string>>(); 
        FindAllAssignments(tasks, 0, new List<string>(), combinations); 

        int count = 1; 
        foreach (var combo in combinations) 
        {
            if (combo.Count == 0) WriteLine($"{count++:D2}. [No tasks assigned]"); 
            else WriteLine($"{count++:D2}. [ {string.Join(", ", combo)} ]"); 
        } 

        WriteLine(new string('-', 45)); 
        WriteLine($"Total combinations: {combinations.Count} (2^{tasks.Count} = {(int)Math.Pow(2, tasks.Count)})"); 
    } 


    // O(2^n) - for each task, we make two choices: 
    //          Include it -> recurse 
    //          Skip it -> recurse 
    //      This doubles the calls at every single step 
    private void FindAllAssignments(
        List<string> tasks, 
        int index, 
        List<string> current, 
        List<List<string>> combinations) 
    {
        if (index == tasks.Count) 
        {
            combinations.Add(new List<string>(current)); 
            return; 
        }

        current.Add(tasks[index]); 
        FindAllAssignments(tasks, index + 1, current, combinations);    // recurse RIGHT 

        current.RemoveAt(current.Count - 1); 
        FindAllAssignments(tasks, index + 1, current, combinations);    // recurse LEFT 
    }
}