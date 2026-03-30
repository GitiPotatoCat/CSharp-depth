using static System.Console; 

namespace _02_TimeSpace_Complexity; 


public class LogarithimicTime 
{
    public void LogTExample() 
    {
        var phoneBook = new List<string> 
        {
            "Aman J", 
            "Bulbul N", 
            "Charlie Brown", 
            "Dravid B", 
            "Edwarn Norton", 
            "Frank A", 
            "Grace H", 
            "Himansha M", 
            "Ivan A", 
            "Jully S"
        }; 

        var searchingName = "Grace H"; 

        WriteLine("Searching PhoneBook"); 
        WriteLine(new string('-', 45)); 

        int resultIndex = FindName(phoneBook, searchingName); 

        if (resultIndex != -1)  WriteLine($"{searchingName} at page {resultIndex + 1}"); 
        else WriteLine($"{searchingName} not found"); 
    } 


    private int FindName(List<string> inputList, string targetName) 
    {
        int left = 0; 
        int right = inputList.Count - 1; 
        int step = 1; 

        while (left <= right) 
        {
            int mid = left + (right - left) / 2; 
            WriteLine($"Step {step++}: Opened page {mid + 1} -> '{inputList[mid]}'"); 

            int comparison = string.Compare(inputList[mid], targetName, StringComparison.Ordinal); 

            if (comparison == 0) {WriteLine($"{targetName} found on mid"); return mid;} 
            else if (comparison < 0) {WriteLine($"{targetName} is AFTER mid, move RIGHT"); left = mid + 1;} 
            else {WriteLine($"{targetName} is BEFORE mid, move LEFT"); right = mid - 1;}
        } 

        return -1;
    }
}